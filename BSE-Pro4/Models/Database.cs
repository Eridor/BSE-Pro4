using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BSE_Pro4.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<Product> Products { get; set; }
    }
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Format { get; set; }
        public string Pages { get; set; }
        public int QuantityAvailable { get; set; }
        public double Cost { get; set; }
        public int TaxId { get; set; }
        public double Discount { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [ForeignKey("AuthorId")]
        public Author Author { get; set; }
        [ForeignKey("TaxId")]
        public Tax Tax { get; set; }
    }
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public string UserID { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("ProductId")]
        public Product ProductItem { get; set; }
        [ForeignKey("UserID")]
        public ApplicationUser User { set; get; }
    }
    public class UserShipment
    {
        [Key]
        public int UserShipId { get; set; }
        public string UserID { get; set; }
        public bool Invoice { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string NumberHouse { get; set; }
        public string NumberFlat { get; set; }
        public string Telephone { get; set; }
        public string AdditionalInfo { get; set; }
        [ForeignKey("UserID")]
        public ApplicationUser User { set; get; }
    }
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public string UserId { get; set; }
        public int? UserShipmentId { get; set; }
        public int? UserInvoiceId { get; set; }
        public double TotalCost { get; set; }
        public int TransactionStatusId { get; set; }
        [ForeignKey("UserShipmentId")]
        public UserShipment UserShipment { get; set; }
        [ForeignKey("UserInvoiceId")]
        public UserShipment UserInvoice { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { set; get; }
        [ForeignKey("TransactionStatusId")]
        public TransactionStatus TransactionStatus { get; set; }
        public List<TransactionItem> TransactionItems { get; set; }
    }
    public class TransactionItem
    {
        public int TransactionItemId { get; set; }
        public int TransactionId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public double Cost { get; set; }
        public double Tax { get; set; }
        public double Discount { get; set; }
        [ForeignKey("TransactionId")]
        public Transaction Transaction { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
    public class TransactionStatus
    {
        public int TransactionStatusId { get; set; }
        public string Description { get; set; }
    }
    public class Tax
    {
        public int TaxId { get; set; }
        public double Value { get; set; }
        public string Description { get; set; }
    }

    public class Database
    {
        ApplicationDbContext _db = new ApplicationDbContext();
        public void LoadExample()
        {
            _db.Authors.RemoveRange(_db.Authors);
            _db.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('Authors', RESEED, 0)");
            _db.Carts.RemoveRange(_db.Carts);
            _db.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('Carts', RESEED, 0)");
            _db.Categories.RemoveRange(_db.Categories);
            _db.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('Categories', RESEED, 0)");
            _db.Products.RemoveRange(_db.Products);
            _db.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('Products', RESEED, 0)");
            _db.Taxes.RemoveRange(_db.Taxes);
            _db.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('Taxes', RESEED, 0)");
            _db.TransactionStatus.RemoveRange(_db.TransactionStatus);
            _db.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('TransactionStatus', RESEED, 0)");
            _db.Transactions.RemoveRange(_db.Transactions);
            _db.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('Transactions', RESEED, 0)");
            _db.TransactionItems.RemoveRange(_db.TransactionItems);
            _db.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('TransactionItems', RESEED, 0)");
            _db.UserShipments.RemoveRange(_db.UserShipments);
            _db.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('UserShipments', RESEED, 0)");

            _db.SaveChanges();

            List<string> kategorie = new List<string>() {"Fantastyka", "Przygodowa", "Kulinarna", "Sensacyjna", "Science-Fiction", "Podróżnicza", "Biografia"};

            foreach (var it in kategorie)
            {
                Category nCategory = new Category();
                nCategory.CategoryName = it;
                _db.Categories.Add(nCategory);
            }

            List<double> podatki = new List<double>() { 0, 0.08, 0.23 };

            foreach (var it in podatki)
            {
                Tax nTax = new Tax();
                nTax.Value = it;
                _db.Taxes.Add(nTax);
            }

            List<string> transstatus = new List<string>() { "Nowe", "Kompletowane", "Wysyłane", "Zakończone" };

            foreach (var it in transstatus)
            {
                TransactionStatus nTS = new TransactionStatus();
                nTS.Description = it;
                _db.TransactionStatus.Add(nTS);
            }


            Author nAuthor = new Author();
            nAuthor.Name = "Tomasz";
            nAuthor.Surname = "Schabik";
            _db.Authors.Add(nAuthor);

            _db.SaveChanges();
            Product nProduct = new Product();
            nProduct.Category = _db.Categories.First(t => t.CategoryName == "Kulinarna");
            nProduct.Author = nAuthor;
            nProduct.Cost = 29.99;
            nProduct.Desc = "Wkrocz w świat pizzy hawajskiej!";
            nProduct.Discount = 0.5;
            nProduct.Format = "Twarda oprawa";
            nProduct.Name = "Z pizzą hawajską wśród zwierząd";
            nProduct.Pages = "1230";
            nProduct.QuantityAvailable = 5;
            nProduct.Tax = _db.Taxes.First(t => t.Value == 0.23);
            _db.Products.Add(nProduct);

            _db.SaveChanges();
        }
    }
}
