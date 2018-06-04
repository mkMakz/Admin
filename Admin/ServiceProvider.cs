using Admin.lib.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Admin.lib.Modules
{
    public class ServiceProvider
    {
        List<Provider> Providers = new List<Provider>();

        List<int> ProvidersPrefix = new List<int>();
        private string path { get; set; }
        public ServiceProvider() : this("")
        {
        }
        public ServiceProvider(string path)
        {
            if (string.IsNullOrEmpty(path))
                this.path = Path.Combine(@"\\dc\Студенты\ПКО\SEB-171.2\C#", "Operators.xml");
            else
                this.path = path;
        }

        public void AddProvider()
        {
            Provider prov = new Provider();

            Console.WriteLine("Emter Company Name");
            prov.NameCompany = Console.ReadLine();

            Console.WriteLine("Enter Logo");
            prov.LigoURL = Console.ReadLine();

            Console.WriteLine("Enter Prosent");
            prov.Procent = Double.Parse(Console.ReadLine());

            Console.WriteLine("Enter LIst prefix" + "For exit press ENTER twis");
            bool exit = true;
            int pre = 0;
            do
            {
                exit = Int32.TryParse(Console.ReadLine(), out pre);
                if (exit && IsExistPrefix(pre))
                {
                    prov.Prefix.Add(pre);
                }
            }
            while (exit);

            if (IsExistProvider(prov))
            {
                Providers.Add(prov);
                ProvidersPrefix.AddRange(prov.Prefix);
                AddProvidetToXML(prov);
            }

        }

        public void EditProvider()
        {
            // 1 найти провайдера
            Console.WriteLine("Enter name provider");
            SearchProviderByNameForEdit(Console.ReadLine());           
        }

        public void DeleteProvider()
        {

        }

        public void SearchProviderByNameForEdit(string name)
        {
            XmlDocument xd = GetDocument();
            XmlElement root = xd.DocumentElement;

            bool find = false;
            foreach (XmlElement item in root)
            {
                find = false;
                foreach (XmlNode i in item.ChildNodes)
                {
                    if (i.Name == "NameCompany" && i.InnerText == name)
                        find = true;
                }
                if (find)
                {
                    XmlElement el = Edit(item);
                    break;
                }
            }
            if (find)
                xd.Save(path);
        }

        private XmlElement Edit(XmlElement prov)
        {
            foreach (XmlElement item in prov.ChildNodes)
            {
                Console.WriteLine(item.Name + ": (" + item.InnerText + ") - ");
                string cn = Console.ReadLine();
                if (!string.IsNullOrEmpty(cn))
                {
                    item.InnerText = cn;
                }
            }
            return prov;
        }

        private bool IsExistProvider(Provider pro)
        {
            if (Providers.Where(w => w.NameCompany == pro.NameCompany).Count() > 0)
            {
                Console.WriteLine("Takoy provider uzhe est");
                return false;
            }
            return true;
        }

        private bool IsExistPrefix(int pref)
        {
            if (ProvidersPrefix.Where(w => w == pref).Count() > 0)
            {
                Console.WriteLine("Takoy prefix est");
                return false;
            }
            return true;

        }

        private void AddProvidetToXML(Provider pro)
        {
            XmlDocument doc = GetDocument();
            XmlElement elem = doc.CreateElement("Provider");

            XmlElement LogoURL = doc.CreateElement("LogoURL");
            LogoURL.InnerText = pro.LigoURL;

            XmlElement NameCompay = doc.CreateElement("NameCompany");
            NameCompay.InnerText = pro.NameCompany;

            XmlElement Procent = doc.CreateElement("Procent");
            Procent.InnerText = pro.Procent.ToString();

            XmlElement Prefixs = doc.CreateElement("Prefixs");
            foreach (var item in pro.Prefix)
            {
                XmlElement Prefix = doc.CreateElement("Prefix");
                Prefix.InnerText = item.ToString();
                Prefixs.AppendChild(Prefix);
            }

            elem.AppendChild(LogoURL);
            elem.AppendChild(NameCompay);
            elem.AppendChild(Procent);
            elem.AppendChild(Prefixs);
            doc.DocumentElement.AppendChild(elem);
            doc.Save(path);


        }

        private XmlDocument GetDocument()
        {
            XmlDocument xd = new XmlDocument();
            // \\dc\Студенты\ПКО\SEB-171.2\C#


            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                xd.Load(path);
            }
            else
            {
                ////1
                //FileStream fs = fi.Create();
                //fs.Close();

                //2

                XmlElement xl = xd.CreateElement("Providers");
                xd.AppendChild(xl);
                xd.Save(path);
            }
            return xd;
        }
    }
}
