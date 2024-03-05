public class Person {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }



// Konstruktør med parametre (Constructor with Parameters)
        public Person(string firstName, string lastName, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }
        public Person() {}
}