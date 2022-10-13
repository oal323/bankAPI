
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bankAPI.Models
{
    public class Account
    {
    
    
    public string id { get; set; }
    public string userid { get; set; }
    public decimal balance { get; set; }
    public int accountType { get; set; }   
    public virtual User User { get; set; }
    public virtual ICollection<Transaction>? transactions { get; set; }
    }
    public class User
  {
    
    
    public string id { get; set; }
    public string last { get; set; }
    public string first { get; set; }
    public string password { get; set; }    
    public string userName { get; set; }
    public virtual ICollection<Account>? AcctsDB { get; set; }
    
  }
  public class Transaction
  {
    
    
    public string id { get; set; }
    public string sourceID { get; set; }
    public string destID { get; set; }
    public decimal amount { get; set; }
    public virtual Account transactionOwner { get; set; }    
  }
  
}