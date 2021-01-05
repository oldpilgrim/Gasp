namespace HelloAspNet.Models
{
    // A model is a set of classes that represent the data that the app manages.
    // Model classes can go anywhere in the project, but the Models folder is used by convention.

    public class TodoItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}