namespace processoINOA {
  public class Stock {
    public string name { get; set; }
    public decimal redLine { get; set; }
    public decimal blueLine { get; set; }
    public int state { get; set; }
    public Stock() { }
    public bool Validate() {
      bool isValid = true;
      isValid &= (redLine < blueLine);
      return isValid;
    }
    public void Format() {
      if (name.Length < 3 || name.Substring(name.Length - 3) != ".SA") {
        name += ".SA";
      }
    }
  }
}