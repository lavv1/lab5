using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatternsDemo
{
    // ==========================
    // 1. Factory Method
    // ==========================
    abstract class User
    {
        public abstract void DisplayRole();
    }

    class Admin : User
    {
        public override void DisplayRole()
        {
            Console.WriteLine("I am an Administrator.");
        }
    }

    class Guest : User
    {
        public override void DisplayRole()
        {
            Console.WriteLine("I am a Guest.");
        }
    }

    class Manager : User
    {
        public override void DisplayRole()
        {
            Console.WriteLine("I am a Manager.");
        }
    }

    abstract class UserCreator
    {
        public abstract User CreateUser();
    }

    class AdminCreator : UserCreator
    {
        public override User CreateUser()
        {
            return new Admin();
        }
    }

    class GuestCreator : UserCreator
    {
        public override User CreateUser()
        {
            return new Guest();
        }
    }

    class ManagerCreator : UserCreator
    {
        public override User CreateUser()
        {
            return new Manager();
        }
    }

    // ==========================
    // 2. Composite
    // ==========================
    abstract class FileSystemComponent
    {
        public string Name { get; set; }

        public FileSystemComponent(string name)
        {
            Name = name;
        }

        public abstract void Display(int depth);
    }

    class File : FileSystemComponent
    {
        public File(string name) : base(name) { }

        public override void Display(int depth)
        {
            Console.WriteLine(new string('-', depth) + Name);
        }
    }

    class Directory : FileSystemComponent
    {
        private List<FileSystemComponent> children = new();

        public Directory(string name) : base(name) { }

        public void Add(FileSystemComponent component)
        {
            children.Add(component);
        }

        public override void Display(int depth)
        {
            Console.WriteLine(new string('-', depth) + Name);
            foreach (var component in children)
            {
                component.Display(depth + 2);
            }
        }
    }

    // ==========================
    // 3. Strategy
    // ==========================
    interface IEncryptionStrategy
    {
        string Encrypt(string data);
    }

    class Base64Encryption : IEncryptionStrategy
    {
        public string Encrypt(string data)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }
    }

    class XorEncryption : IEncryptionStrategy
    {
        private readonly byte key = 0x5A;

        public string Encrypt(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] ^= key;
            }
            return Convert.ToBase64String(bytes);
        }
    }

    class ReverseEncryption : IEncryptionStrategy
    {
        public string Encrypt(string data)
        {
            var charArray = data.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }

    class Encryptor
    {
        private IEncryptionStrategy strategy;

        public void SetStrategy(IEncryptionStrategy strategy)
        {
            this.strategy = strategy;
        }

        public void EncryptData(string data)
        {
            if (strategy == null)
            {
                Console.WriteLine("Encryption strategy not set.");
                return;
            }

            Console.WriteLine("Original: " + data);
            Console.WriteLine("Encrypted: " + strategy.Encrypt(data));
        }
    }

    // ==========================
    // Main
    // ==========================
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("=== Factory Method ===");
            UserCreator adminCreator = new AdminCreator();
            UserCreator guestCreator = new GuestCreator();
            UserCreator managerCreator = new ManagerCreator();

            User admin = adminCreator.CreateUser();
            User guest = guestCreator.CreateUser();
            User manager = managerCreator.CreateUser();

            admin.DisplayRole();
            guest.DisplayRole();
            manager.DisplayRole();

            Console.WriteLine("\n=== Composite (File System) ===");
            Directory root = new Directory("Root");
            Directory documents = new Directory("Documents");
            documents.Add(new File("Resume.docx"));
            documents.Add(new File("Report.pdf"));

            Directory images = new Directory("Images");
            images.Add(new File("photo1.jpg"));
            images.Add(new File("logo.png"));

            root.Add(documents);
            root.Add(images);
            root.Add(new File("README.txt"));

            root.Display(1);
        }
    }
}
