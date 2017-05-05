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
        public float Cost { get; set; }
        public int TaxId { get; set; }
        public float Discount { get; set; }
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
        public int UserShipmentId { get; set; }
        public int? UserInvoiceId { get; set; }
        public float TotalCost { get; set; }
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
        public float Cost { get; set; }
        public float Tax { get; set; }
        public float Discount { get; set; }
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

}
