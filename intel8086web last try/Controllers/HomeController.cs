using intel8086web_last_try.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace intel8086web_last_try.Controllers
{
    public class intel
    {
        public static string[,] registers = { { "AL", "AH", "BL", "BH", "CL", "CH", "DL", "DH" }, { "", "", "", "", "", "", "", "", } };
        public static string[] memory = new string[65536];
    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public static void checklen(ref string x)
        {
            if (x.Length > 2)
            {
                x = x.Remove(0, 1);
                checklen(ref x);
            }
            if (x.Length < 2)
            {
                x = x.Insert(0, "0");
                checklen(ref x);
            }
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult enter(regModel model)
        {
            intel.registers[1, 0] = model.AL;
            intel.registers[1, 1] = model.AH;
            intel.registers[1, 2] = model.BL;
            intel.registers[1, 3] = model.BH;
            intel.registers[1, 4] = model.CL;
            intel.registers[1, 5] = model.CH;
            intel.registers[1, 6] = model.DL;
            intel.registers[1, 7] = model.DH;





            ViewData["AL"] = intel.registers[1, 0];
            ViewData["AH"] = intel.registers[1, 1];
            ViewData["BL"] = intel.registers[1, 2];
            ViewData["BH"] = intel.registers[1, 3];
            ViewData["CL"] = intel.registers[1, 4];
            ViewData["CH"] = intel.registers[1, 5];
            ViewData["DL"] = intel.registers[1, 6];
            ViewData["DH"] = intel.registers[1, 7];

            return View("Index", model);
        }

        public IActionResult execut(regModel model)
        {


            switch (model.fc)
            {
                case "MOV":
                    {
                        MOV(model.index1,model.index2);
                        break;
                    }
                case "XCHG":
                    {
                        XCHG(model.index1, model.index2);
                        break;
                    }
                case "ADD":
                    {
                        ADD(model.index1, model.index2);
                        break;
                    }
                case "INC":
                    {
                        INC(model.index1);
                        break;
                    }
                case "DEC":
                    {
                        DEC(model.index1);
                        break;
                    }
                case "SUB":
                    {
                        SUB(model.index1, model.index2);
                        break;
                    }
                case "NOT":
                    {
                        NOT(model.index1);
                        break;
                    }
                case "AND":
                    {
                        AND(model.index1, model.index2);
                        break;
                    }
                case "OR":
                    {
                        OR(model.index1, model.index2);
                        break;
                    }
                case "XOR":
                    {
                        XOR(model.index1, model.index2);
                        break;
                    }
                case "RES":
                    {
                        RES();
                        break;
                    }
            }



            ViewData["AL"] = intel.registers[1, 0];
            ViewData["AH"] = intel.registers[1, 1];
            ViewData["BL"] = intel.registers[1, 2];
            ViewData["BH"] = intel.registers[1, 3];
            ViewData["CL"] = intel.registers[1, 4];
            ViewData["CH"] = intel.registers[1, 5];
            ViewData["DL"] = intel.registers[1, 6];
            ViewData["DH"] = intel.registers[1, 7];

            return View("Index");
        }






        public static void RES()
        {
            for (int i = 0; i < intel.registers.GetLength(1); i++)
            {
                intel.registers[1, i] = "";
            }
            Array.Clear(intel.memory);
            return;
        }


        public static void AND(string reg1, string reg2)
        {

            int index1 = 0;
            int tempreg1 = 0;
            int index2 = 0;
            int tempreg2 = 0;
            if (reg2 == "AL" ||
                reg2 == "AH" ||
                reg2 == "BL" ||
                reg2 == "BH" ||
                reg2 == "CL" ||
                reg2 == "CH" ||
                reg2 == "DL" ||
                reg2 == "DH")
            {
                for (int i = 0; i < intel.registers.GetLength(1); i++)
                {
                    if (intel.registers[0, i] == reg2)
                    {
                        index2 = i;
                        tempreg2 = int.Parse(intel.registers[1, index2], NumberStyles.AllowHexSpecifier);
                    }

                }

            }
            else
            {
                int regm2 = int.Parse(reg2, NumberStyles.AllowHexSpecifier);
                tempreg2 = int.Parse(intel.memory[regm2], NumberStyles.AllowHexSpecifier);
            }
            if (reg1 == "AL" ||
                reg1 == "AH" ||
                reg1 == "BL" ||
                reg1 == "BH" ||
                reg1 == "CL" ||
                reg1 == "CH" ||
                reg1 == "DL" ||
                reg1 == "DH")
            {
                for (int i = 0; i < intel.registers.GetLength(1); i++)
                {
                    if (intel.registers[0, i] == reg1)
                    {
                        index1 = i;
                        tempreg1 = int.Parse(intel.registers[1, index1], NumberStyles.AllowHexSpecifier);
                    }

                }
                int and = tempreg1 & tempreg2;
                intel.registers[1, index1] = Convert.ToString(Convert.ToInt32(and), 16).ToUpper();
                checklen(ref intel.registers[1, index1]);
            }
            else
            {
                int regm1 = int.Parse(reg1, NumberStyles.AllowHexSpecifier);
                tempreg1 = int.Parse(intel.memory[regm1], NumberStyles.AllowHexSpecifier);
                int and = tempreg1 & tempreg2;
                intel.memory[regm1] = Convert.ToString(Convert.ToInt32(and), 16).ToUpper();
                checklen(ref intel.memory[regm1]);
            }

            return;
        }
        public static void OR(string reg1, string reg2)
        {


            int index1 = 0;
            int tempreg1 = 0;
            int index2 = 0;
            int tempreg2 = 0;
            if (reg2 == "AL" ||
                reg2 == "AH" ||
                reg2 == "BL" ||
                reg2 == "BH" ||
                reg2 == "CL" ||
                reg2 == "CH" ||
                reg2 == "DL" ||
                reg2 == "DH")
            {
                for (int i = 0; i < intel.registers.GetLength(1); i++)
                {
                    if (intel.registers[0, i] == reg2)
                    {
                        index2 = i;
                        tempreg2 = int.Parse(intel.registers[1, index2], NumberStyles.AllowHexSpecifier);
                    }

                }

            }
            else
            {
                int regm2 = int.Parse(reg2, NumberStyles.AllowHexSpecifier);
                tempreg2 = int.Parse(intel.memory[regm2], NumberStyles.AllowHexSpecifier);
            }
            if (reg1 == "AL" ||
                reg1 == "AH" ||
                reg1 == "BL" ||
                reg1 == "BH" ||
                reg1 == "CL" ||
                reg1 == "CH" ||
                reg1 == "DL" ||
                reg1 == "DH")
            {
                for (int i = 0; i < intel.registers.GetLength(1); i++)
                {
                    if (intel.registers[0, i] == reg1)
                    {
                        index1 = i;
                        tempreg1 = int.Parse(intel.registers[1, index1], NumberStyles.AllowHexSpecifier);
                    }

                }
                int or = tempreg1 | tempreg2;
                intel.registers[1, index1] = Convert.ToString(Convert.ToInt32(or), 16).ToUpper();
                checklen(ref intel.registers[1, index1]);
            }
            else
            {
                int regm1 = int.Parse(reg1, NumberStyles.AllowHexSpecifier);
                tempreg1 = int.Parse(intel.memory[regm1], NumberStyles.AllowHexSpecifier);
                int or = tempreg1 | tempreg2;
                intel.memory[regm1] = Convert.ToString(Convert.ToInt32(or), 16).ToUpper();
                checklen(ref intel.memory[regm1]);
            }

            return;
        }
        public static void XOR(string reg1, string reg2)
        {

            int index1 = 0;
            int tempreg1 = 0;
            int index2 = 0;
            int tempreg2 = 0;
            if (reg2 == "AL" ||
                reg2 == "AH" ||
                reg2 == "BL" ||
                reg2 == "BH" ||
                reg2 == "CL" ||
                reg2 == "CH" ||
                reg2 == "DL" ||
                reg2 == "DH")
            {
                for (int i = 0; i < intel.registers.GetLength(1); i++)
                {
                    if (intel.registers[0, i] == reg2)
                    {
                        index2 = i;
                        tempreg2 = int.Parse(intel.registers[1, index2], NumberStyles.AllowHexSpecifier);
                    }

                }

            }
            else
            {
                int regm2 = int.Parse(reg2, NumberStyles.AllowHexSpecifier);
                tempreg2 = int.Parse(intel.memory[regm2], NumberStyles.AllowHexSpecifier);
            }
            if (reg1 == "AL" ||
                reg1 == "AH" ||
                reg1 == "BL" ||
                reg1 == "BH" ||
                reg1 == "CL" ||
                reg1 == "CH" ||
                reg1 == "DL" ||
                reg1 == "DH")
            {
                for (int i = 0; i < intel.registers.GetLength(1); i++)
                {
                    if (intel.registers[0, i] == reg1)
                    {
                        index1 = i;
                        tempreg1 = int.Parse(intel.registers[1, index1], NumberStyles.AllowHexSpecifier);
                    }

                }
                int xor = tempreg1 ^ tempreg2;
                intel.registers[1, index1] = Convert.ToString(Convert.ToInt32(xor), 16).ToUpper();
                checklen(ref intel.registers[1, index1]);
            }
            else
            {
                int regm1 = int.Parse(reg1, NumberStyles.AllowHexSpecifier);
                tempreg1 = int.Parse(intel.memory[regm1], NumberStyles.AllowHexSpecifier);
                int xor = tempreg1 ^ tempreg2;
                intel.memory[regm1] = Convert.ToString(Convert.ToInt32(xor), 16).ToUpper();
                checklen(ref intel.memory[regm1]);
            }

            return;
        }

        public static void SUB(string reg1, string reg2)
        {


            int index1 = 0;
            int tempreg1 = 0;
            int index2 = 0;
            int tempreg2 = 0;
            if (reg2 == "AL" ||
                reg2 == "AH" ||
                reg2 == "BL" ||
                reg2 == "BH" ||
                reg2 == "CL" ||
                reg2 == "CH" ||
                reg2 == "DL" ||
                reg2 == "DH")
            {
                for (int i = 0; i < intel.registers.GetLength(1); i++)
                {
                    if (intel.registers[0, i] == reg2)
                    {
                        index2 = i;
                        tempreg2 = int.Parse(intel.registers[1, index2], NumberStyles.AllowHexSpecifier);
                    }

                }

            }
            else
            {
                int regm2 = int.Parse(reg2, NumberStyles.AllowHexSpecifier);
                tempreg2 = int.Parse(intel.memory[regm2], NumberStyles.AllowHexSpecifier);
            }
            if (reg1 == "AL" ||
                reg1 == "AH" ||
                reg1 == "BL" ||
                reg1 == "BH" ||
                reg1 == "CL" ||
                reg1 == "CH" ||
                reg1 == "DL" ||
                reg1 == "DH")
            {
                for (int i = 0; i < intel.registers.GetLength(1); i++)
                {
                    if (intel.registers[0, i] == reg1)
                    {
                        index1 = i;
                        tempreg1 = int.Parse(intel.registers[1, index1], NumberStyles.AllowHexSpecifier);
                    }

                }
                int sub = tempreg1 - tempreg2;
                intel.registers[1, index1] = Convert.ToString(Convert.ToInt32(sub), 16).ToUpper();
                checklen(ref intel.registers[1, index1]);
            }
            else
            {
                int regm1 = int.Parse(reg1, NumberStyles.AllowHexSpecifier);
                tempreg1 = int.Parse(intel.memory[regm1], NumberStyles.AllowHexSpecifier);
                int sub = tempreg1 - tempreg2;
                intel.memory[regm1] = Convert.ToString(Convert.ToInt32(sub), 16).ToUpper();
                checklen(ref intel.memory[regm1]);
            }

            return;
        }
        public static void NOT(string reg1)
        {


            int index1 = 0;
            int tempreg1 = 0;
            if (reg1 == "AL" ||
                reg1 == "AH" ||
                reg1 == "BL" ||
                reg1 == "BH" ||
                reg1 == "CL" ||
                reg1 == "CH" ||
                reg1 == "DL" ||
                reg1 == "DH")
            {
                for (int i = 0; i < intel.registers.GetLength(1); i++)
                {
                    if (intel.registers[0, i] == reg1)
                    {
                        index1 = i;
                        tempreg1 = int.Parse(intel.registers[1, index1], NumberStyles.AllowHexSpecifier);
                    }

                }
                int not = 255 - tempreg1 ;
                intel.registers[1, index1] = Convert.ToString(Convert.ToInt32(not), 16).ToUpper();
                checklen(ref intel.registers[1, index1]);
            }
            else
            {
                int regm1 = int.Parse(reg1, NumberStyles.AllowHexSpecifier);
                tempreg1 = int.Parse(intel.memory[regm1], NumberStyles.AllowHexSpecifier);
                int not = 255 - tempreg1 ;
                intel.memory[regm1] = Convert.ToString(Convert.ToInt32(not), 16).ToUpper();
                checklen(ref intel.memory[regm1]);
            }
            return;
        }
        public static void ADD(string reg1, string reg2)
        {

            int index1 = 0;
            int tempreg1 = 0;
            int index2 = 0;
            int tempreg2 = 0;
            if (reg2 == "AL" ||
                reg2 == "AH" ||
                reg2 == "BL" ||
                reg2 == "BH" ||
                reg2 == "CL" ||
                reg2 == "CH" ||
                reg2 == "DL" ||
                reg2 == "DH")
            {
                for (int i = 0; i < intel.registers.GetLength(1); i++)
                {
                    if (intel.registers[0, i] == reg2)
                    {
                        index2 = i;
                        tempreg2 = int.Parse(intel.registers[1, index2], NumberStyles.AllowHexSpecifier);
                    }

                }

            }
            else
            {
                int regm2 = int.Parse(reg2, NumberStyles.AllowHexSpecifier);
                tempreg2 = int.Parse(intel.memory[regm2], NumberStyles.AllowHexSpecifier);
            }
            if (reg1 == "AL" ||
                reg1 == "AH" ||
                reg1 == "BL" ||
                reg1 == "BH" ||
                reg1 == "CL" ||
                reg1 == "CH" ||
                reg1 == "DL" ||
                reg1 == "DH")
            {
                for (int i = 0; i < intel.registers.GetLength(1); i++)
                {
                    if (intel.registers[0, i] == reg1)
                    {
                        index1 = i;
                        tempreg1 = int.Parse(intel.registers[1, index1], NumberStyles.AllowHexSpecifier);
                    }

                }
                int add = tempreg1 + tempreg2;
                intel.registers[1, index1] = Convert.ToString(Convert.ToInt32(add), 16).ToUpper();
                checklen(ref intel.registers[1, index1]);
            }
            else
            {
                int regm1 = int.Parse(reg1, NumberStyles.AllowHexSpecifier);
                tempreg1 = int.Parse(intel.memory[regm1], NumberStyles.AllowHexSpecifier);
                int add = tempreg1 + tempreg2;
                intel.memory[regm1] = Convert.ToString(Convert.ToInt32(add), 16).ToUpper();
                checklen(ref intel.memory[regm1]);
            }

            return;
        }
        public static void INC(string reg1)
        {


            int index1 = 0;
            int tempreg1 = 0;
            if (reg1 == "AL" ||
                reg1 == "AH" ||
                reg1 == "BL" ||
                reg1 == "BH" ||
                reg1 == "CL" ||
                reg1 == "CH" ||
                reg1 == "DL" ||
                reg1 == "DH")
            {
                for (int i = 0; i < intel.registers.GetLength(1); i++)
                {
                    if (intel.registers[0, i] == reg1)
                    {
                        index1 = i;
                        tempreg1 = int.Parse(intel.registers[1, index1], NumberStyles.AllowHexSpecifier);
                    }

                }
                tempreg1++;
                intel.registers[1, index1] = Convert.ToString(Convert.ToInt32(tempreg1), 16).ToUpper();
                checklen(ref intel.registers[1, index1]);
            }
            else
            {
                int regm1 = int.Parse(reg1, NumberStyles.AllowHexSpecifier);
                tempreg1 = int.Parse(intel.memory[regm1], NumberStyles.AllowHexSpecifier);
                tempreg1++;
                intel.memory[regm1] = Convert.ToString(Convert.ToInt32(tempreg1), 16).ToUpper();
                checklen(ref intel.memory[regm1]);
            }
            return;
        }
        public static void DEC(string reg1)
        {

            int index1 = 0;
            int tempreg1 = 0;
            if (reg1 == "AL" ||
                reg1 == "AH" ||
                reg1 == "BL" ||
                reg1 == "BH" ||
                reg1 == "CL" ||
                reg1 == "CH" ||
                reg1 == "DL" ||
                reg1 == "DH")
            {
                for (int i = 0; i < intel.registers.GetLength(1); i++)
                {
                    if (intel.registers[0, i] == reg1)
                    {
                        index1 = i;
                        tempreg1 = int.Parse(intel.registers[1, index1], NumberStyles.AllowHexSpecifier);
                    }

                }
                tempreg1--;
                intel.registers[1, index1] = Convert.ToString(Convert.ToInt32(tempreg1), 16).ToUpper();
                checklen(ref intel.registers[1, index1]);
            }
            else
            {
                int regm1 = int.Parse(reg1, NumberStyles.AllowHexSpecifier);
                tempreg1 = int.Parse(intel.memory[regm1], NumberStyles.AllowHexSpecifier);
                tempreg1--;
                intel.memory[regm1] = Convert.ToString(Convert.ToInt32(tempreg1), 16).ToUpper();
                checklen(ref intel.memory[regm1]);
            }
            return;
        }

        public static void MOV(string reg1, string reg2)
        {


            int index1 = 0;
            int index2 = 0;
            string tempreg = "";
            if (reg2 == "AL" ||
                reg2 == "AH" ||
                reg2 == "BL" ||
                reg2 == "BH" ||
                reg2 == "CL" ||
                reg2 == "CH" ||
                reg2 == "DL" ||
                reg2 == "DH")
            {
                for (int i = 0; i < intel.registers.GetLength(1); i++)
                {
                    if (intel.registers[0, i] == reg2)
                    {
                        index2 = i;
                        tempreg = intel.registers[1, index2];
                    }

                }

            }
            else
            {
                int regm2 = int.Parse(reg2, NumberStyles.AllowHexSpecifier);
                tempreg = intel.memory[regm2];
            }
            if (reg1 == "AL" ||
                reg1 == "AH" ||
                reg1 == "BL" ||
                reg1 == "BH" ||
                reg1 == "CL" ||
                reg1 == "CH" ||
                reg1 == "DL" ||
                reg1 == "DH")
            {
                for (int i = 0; i < intel.registers.GetLength(1); i++)
                {
                    if (intel.registers[0, i] == reg1)
                    {
                        index1 = i;                   
                    }

                }
                intel.registers[1, index1] = tempreg;
            }
            else
            {
                int regm1 = int.Parse(reg1, NumberStyles.AllowHexSpecifier);
                intel.memory[regm1] = tempreg;
            }
            return;
        }
        public static void XCHG(string reg1, string reg2)
        {
            int index1 = 0;
            int index2 = 0;
            string tempreg1 = "";
            string tempreg2 = "";
            if (reg2 == "AL" ||
                reg2 == "AH" ||
                reg2 == "BL" ||
                reg2 == "BH" ||
                reg2 == "CL" ||
                reg2 == "CH" ||
                reg2 == "DL" ||
                reg2 == "DH")
            {
                for (int i = 0; i < intel.registers.GetLength(1); i++)
                {
                    if (intel.registers[0, i] == reg2)
                    {
                        index2 = i;
                        tempreg2 = intel.registers[1, index2];
                    }

                }

            }
            else
            {
                int regm2 = int.Parse(reg2, NumberStyles.AllowHexSpecifier);
                tempreg2 = intel.memory[regm2];
            }
            if (reg1 == "AL" ||
                reg1 == "AH" ||
                reg1 == "BL" ||
                reg1 == "BH" ||
                reg1 == "CL" ||
                reg1 == "CH" ||
                reg1 == "DL" ||
                reg1 == "DH")
            {
                for (int i = 0; i < intel.registers.GetLength(1); i++)
                {
                    if (intel.registers[0, i] == reg1)
                    {
                        index1 = i;
                    }

                }
                tempreg1 = intel.registers[1, index1];
                intel.registers[1, index1] = tempreg2;
            }
            else
            {
                int regm1 = int.Parse(reg1, NumberStyles.AllowHexSpecifier);
                tempreg1 = intel.memory[regm1];
                intel.memory[regm1] = tempreg2;
            }
            if (reg2 == "AL" ||
                reg2 == "AH" ||
                reg2 == "BL" ||
                reg2 == "BH" ||
                reg2 == "CL" ||
                reg2 == "CH" ||
                reg2 == "DL" ||
                reg2 == "DH")
            {
                intel.registers[1, index2] = tempreg1;
            }
            else
            {
                int regm2 = int.Parse(reg2, NumberStyles.AllowHexSpecifier);
                intel.memory[regm2] = tempreg1;
            }

            return;
        }



        [HttpPost]
        public IActionResult insert(regModel model)
        {
            int temp = int.Parse(model.index3, NumberStyles.AllowHexSpecifier); ;

            intel.memory[temp] = model.index4;


            ViewData["AL"] = intel.registers[1, 0];
            ViewData["AH"] = intel.registers[1, 1];
            ViewData["BL"] = intel.registers[1, 2];
            ViewData["BH"] = intel.registers[1, 3];
            ViewData["CL"] = intel.registers[1, 4];
            ViewData["CH"] = intel.registers[1, 5];
            ViewData["DL"] = intel.registers[1, 6];
            ViewData["DH"] = intel.registers[1, 7];
            return View("Index");
        }

        [HttpPost]
        public IActionResult show(regModel model)
        {
            int temp = int.Parse(model.index5, NumberStyles.AllowHexSpecifier); ;
            
            ViewData["show"] = intel.memory[temp];


            ViewData["AL"] = intel.registers[1, 0];
            ViewData["AH"] = intel.registers[1, 1];
            ViewData["BL"] = intel.registers[1, 2];
            ViewData["BH"] = intel.registers[1, 3];
            ViewData["CL"] = intel.registers[1, 4];
            ViewData["CH"] = intel.registers[1, 5];
            ViewData["DL"] = intel.registers[1, 6];
            ViewData["DH"] = intel.registers[1, 7];
            return View("Index");
        }





        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}