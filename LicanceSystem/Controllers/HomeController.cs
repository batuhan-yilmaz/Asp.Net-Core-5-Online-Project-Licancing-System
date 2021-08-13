using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LicanceSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            LisansKontrol();
            if (ViewBag.LisansKontrol == "true")
            {
                return RedirectToAction("ErrorLicance", "Home");
            }
           
            return View();

        }

        public IActionResult ErrorLicance()
        {
            return View();
        }

        private void LisansKontrol()
        {
            string siteAdresi = Request.Host.ToString().TrimEnd('/');
            string LisansAdres = "";
            string Versiyon = "";
            string LisansKodu = "";
            string LisansBaslangicTarihi = "";
            string BitisTarihi = "";
            string BaslangicTarihi = DateTime.Now.ToString("dd.MM.yyyy");
            string[] AdresParcala, VersiyonParcala, BaslangicTarihiParcala, BitisTarihiParcala, LisansNoParcala;


            //string Url = "https://www.example.com/Licance.xml"; // Licance Source remote server check
            string Url = "Licance.xml"; // localhost check
            XElement xEmp = XElement.Load(Url);
            var empNames = from name in xEmp.Elements("Site")
                           select name;

            foreach (XElement fName in empNames)
            {
                LisansAdres += fName.Element("SiteAdi").Value.ToString().TrimEnd('/') + ",";
                Versiyon += fName.Element("Versiyon").Value + ",";
                LisansKodu += fName.Element("LisansNo").Value + ",";
                LisansBaslangicTarihi += fName.Element("BaslangicTarihi").Value + ",";
                BitisTarihi += fName.Element("BitisTarihi").Value + ",";
            }

            LisansAdres = LisansAdres.TrimEnd(',');
            Versiyon = Versiyon.TrimEnd(',');
            LisansBaslangicTarihi = LisansBaslangicTarihi.TrimEnd(',');
            BitisTarihi = BitisTarihi.TrimEnd(',');
            LisansKodu = LisansKodu.TrimEnd(',');

            AdresParcala = LisansAdres.Split(',');
            VersiyonParcala = Versiyon.Split(',');
            BaslangicTarihiParcala = LisansBaslangicTarihi.Split(',');
            BitisTarihiParcala = BitisTarihi.Split(',');
            LisansNoParcala = LisansKodu.Split(',');

            bool Kontrol = false;

            for (int i = 0; i < AdresParcala.Length; i++)
            {
                if (siteAdresi == AdresParcala[i].ToString())
                {
                    Kontrol = true;
                    i = AdresParcala.Length;
                }
                else
                {
                    Kontrol = false;
                }
            }

            if (Kontrol)
            {
                for (int i = 0; i < AdresParcala.Length; i++)
                {
                    if (siteAdresi == AdresParcala[i].ToString())
                    {
                        var BTarih = Convert.ToDateTime(BaslangicTarihi);
                        var BitTarih = Convert.ToDateTime(BitisTarihiParcala[i].ToString());

                        if (BitTarih >= BTarih)
                        {
                            ViewBag.SiteAdi = AdresParcala[i].ToString();
                            ViewBag.Versiyon = VersiyonParcala[i].ToString();
                            ViewBag.BaslangicTarihi = BaslangicTarihiParcala[i].ToString();
                            ViewBag.BitisTarihi = BitisTarihiParcala[i].ToString();
                            ViewBag.LisansKodu = LisansNoParcala[i].ToString();
                            i = AdresParcala.Length;
                        }
                        else
                        {
                            ViewBag.LisansKontrol = Convert.ToString("true");
                        }
                    }
                }
            }
            else
            {
                ViewBag.LisansKontrol = Convert.ToString("true");
            }
        }
    }
}
