﻿using AttendanceProAPI.Models;
using AttendanceProAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceProAPI.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private SendGridSettings settings;
        private CloudBlobClient blobClient;
        private CloudStorageAccount storageAccount;
        private CloudBlobContainer blobContainer;
        public BlobStorageService(IOptions<SendGridSettings> settings)
        {
            this.settings = settings.Value;
            storageAccount = CloudStorageAccount.Parse(this.settings.BlobConnectionString);
            blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference(this.settings.BlobContainer);
        }
        /// <summary>
        /// This method is used to add new emails to the linked student container in blob storage.
        /// </summary>
        public async Task<IActionResult> AddNewEmailData(SendGridEmailRequest email, string folder, string file)
        {
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference($"{folder}/{file}");
            await blob.UploadTextAsync(JsonConvert.SerializeObject(email));
            return new OkResult();
        }
        /// <summary>
        /// This method is used to retrieve emails from the linked student container (blob storage).
        /// </summary>
        public async Task<List<SendGridEmailRequest>> GetEmails(string folder)
        {
            List<SendGridEmailRequest> emails = new List<SendGridEmailRequest>();
            BlobContinuationToken blobContinuationToken = null;
            BlobResultSegment blobResponse = await blobContainer.ListBlobsSegmentedAsync($"{folder}/", true, BlobListingDetails.None, null, blobContinuationToken, null, null);
            foreach (CloudBlockBlob blob in blobResponse.Results.OfType<CloudBlockBlob>())
            {
                MemoryStream memoryStream = new MemoryStream();
                await blob.DownloadToStreamAsync(memoryStream);
                emails.Add(JsonConvert.DeserializeObject<SendGridEmailRequest>(System.Text.Encoding.UTF8.GetString(memoryStream.ToArray())));
            }
            return emails;
        }
    }
}
