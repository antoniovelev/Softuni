﻿using Panda.Data;
using Panda.Data.Models;
using Panda.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Panda.Services
{
    public class PackagesService : IPackagesService
    {
        private readonly PandaDbContext db;
        private readonly IReceiptsService receiptsService;

        public PackagesService(PandaDbContext db, IReceiptsService receiptsService)
        {
            this.db = db;
            this.receiptsService = receiptsService;
        }
        public void Create(string description, decimal weight, string shippingAddress, string recipientName)
        {
            var userId = db.Users.Where(x => x.Username == recipientName).Select(x => x.Id).FirstOrDefault();

            if (userId == null)
            {
                return;
            }
            var package = new Package
            {
                Description = description,
                Weight = weight,
                Status = Status.Pending,
                ShippingAddress = shippingAddress,
                RecipientId = userId
            };

            this.db.Packages.Add(package);
            this.db.SaveChanges();
        }

        public void Deliver(string id)
        {
            var package = this.db.Packages.FirstOrDefault(x => x.Id == id);
            if (package == null)
            {
                return;
            }

            package.Status = Status.Delivered;
            this.db.SaveChanges();

            this.receiptsService.CreateFromPackage(package.Weight, package.Id, package.RecipientId);
        }

        public IQueryable<Package> GetAllByStatus(Status status)
        {
            var packages = db.Packages.Where(x => x.Status == status);

            return packages;
        }
    }
}
