using System;
using System.Diagnostics;

namespace OS_Test2
{
    class VirtualiMasina
    {
        public static string CMDKomanda;
        public static void CMDRezimas()
        {
            RealiMasina.MODE = 1;                                                        //Keiciamas registras MODE
            Console.WriteLine("Pradedama virtuali masina CMD režimu");
            CMDKomanda = Console.ReadLine();
            var kint = CMDKomanda.Split(' ');
            while (kint[0] != "HALT")              //Cia visada veikia Virtuali masina iki HALT
            {
                if (kint.Length > 2)
                {
                    Console.WriteLine("Ivesta komanda:" + kint[0]);
                    RealiMasina.IC++;
                    int numVal1 = Int32.Parse(kint[0]);
                    int numVal2 = Int32.Parse(kint[1]);
                    int numVal3 = Int32.Parse(kint[2]);
                    Commands.a = numVal1;
                    RealiMasina.IOI = 1;
                    RealiMasina.CH1 = 1;
                    Commands.Vykdom(numVal2, numVal3);
                    RealiMasina.CH1 = 0;
                    RealiMasina.IOI = 0;

                    RealiMasina.PertraukimuTikrinimas();
                    CMDKomanda = Console.ReadLine();
                    kint = CMDKomanda.Split(' ');
                }
                else {
                    Console.WriteLine("Įvesta per mažai argumentų");
                    RealiMasina.IC++;
                    RealiMasina.PI = 1;
                    RealiMasina.PertraukimuTikrinimas();
                }
                if (RealiMasina.MODE == 0) {
                    kint[0] = "HALT";
                }

            }
            Console.WriteLine("Vistuali masina sustabdoma");
        }
        public static void VRezimas()
        {
            RealiMasina.MODE = 1;                                                        //Keiciamas registras MODE
            Console.WriteLine("Pradedama virtuali masina su komandomis is failo: " + RealiMasina.FailoVardas[1]);
            string VMtext = System.IO.File.ReadAllText(@"C:\Users\Witcher\Documents\Visual Studio 2015\Projects\OS_Test2\" + RealiMasina.FailoVardas[1]);

            var VirtualiosMasinosKomandos = VMtext.Split('\n');
            int numVal1 = Int32.Parse(VirtualiosMasinosKomandos[0]);
            int numVal2 = Int32.Parse(VirtualiosMasinosKomandos[1]);
            int numVal3 = Int32.Parse(VirtualiosMasinosKomandos[2]);
            Commands.a = numVal1;
            Commands.Vykdom(numVal2, numVal3);
            foreach (var a in VirtualiosMasinosKomandos)
            {
                Console.WriteLine(a);
                RealiMasina.IC++;
            }

        }
    }

    class Commands
    {
        public static int a;
        public static void Vykdom(int zodis1, int zodis2)
        {
            switch (a)
            {
			case 1:
				Console.WriteLine("Vykdoma komanda GIV");
				if (zodis1 > -1)
				{
					RealiMasina.memory[zodis1] = zodis2; //Cia GIV i rasoma i memory reiksme
				} else {
					Console.WriteLine("Priskiriama registro reiksme"); //Cia GIV i rasoma i registra reiksme (dabar neveikia, sutvarkyk)
				}
				break;
			case 2:
				Console.WriteLine("Vykdoma komanda ADD");
				RealiMasina.memory[zodis1] = RealiMasina.memory[zodis1] + RealiMasina.memory[zodis2];
				break;
			case 3:
				Console.WriteLine("Vykdoma komanda SUB");
				RealiMasina.memory[zodis1] = RealiMasina.memory[zodis1] - RealiMasina.memory[zodis2];
				break;
			case 4:
				Console.WriteLine("Vykdoma komanda MUL");
				RealiMasina.memory[zodis1] = RealiMasina.memory[zodis1] * RealiMasina.memory[zodis2];
				break;
			case 5:
				Console.WriteLine("Vykdoma komanda DIV");
				if (zodis2 == 0)
				{
					RealiMasina.PI = 1;
				} else {
					RealiMasina.memory[zodis1] = RealiMasina.memory[zodis1] / RealiMasina.memory[zodis2];
				}
				break;
			case 6:
				Console.WriteLine("Vykdoma komanda CPM");
				if (RealiMasina.R1 == 0)
				{
					RealiMasina.C = 1;
				}
				if (RealiMasina.R1 < 0)
				{
					RealiMasina.C = 0;
				}
				if (RealiMasina.R1 > 0)
				{
					RealiMasina.C = 2;
				}
				break;
			case 7:
				Console.WriteLine ("Vykdoma komanda AND");
				char[] bin1 = Convert.ToString (zodis1, 2).PadLeft (4, '0').ToCharArray ();
				char[] bin2 = Convert.ToString (zodis2, 2).PadLeft (4, '0').ToCharArray ();
				string result = "";
				for (int i = 0; i < bin1.Length; i++) {
					if ((bin1 [i] == '1') && (bin2 [i] == '1')) {
						result = result + '1';
					} else {
						result = result + '0';
					}
				}
				int resultint = Convert.ToInt32 (result, 2);
				RealiMasina.memory [zodis1] = resultint;
				break;
			case 8:
				Console.WriteLine("Vykdoma komanda OR");
				char[] bin1 = Convert.ToString (zodis1, 2).PadLeft (4, '0').ToCharArray ();
				char[] bin2 = Convert.ToString (zodis2, 2).PadLeft (4, '0').ToCharArray ();
				string result = "";
				for (int i = 0; i < bin1.Length; i++) {
					if ((bin1 [i] == '0') && (bin2 [i] == '0')) {
						result = result + '0';
					} else {
						result = result + '1';
					}
				}
				int resultint = Convert.ToInt32 (result, 2);
				RealiMasina.memory [zodis1] = resultint;
				break;
			case 9:
				Console.WriteLine("Vykdoma komanda XOR");
				char[] bin1 = Convert.ToString (zodis1, 2).PadLeft (4, '0').ToCharArray ();
				char[] bin2 = Convert.ToString (zodis2, 2).PadLeft (4, '0').ToCharArray ();
				string result = "";
				for (int i = 0; i < bin1.Length; i++) {
					if ((bin1 [i] + bin2[i]) == '1')
					{
						result = result + '1';
					} else {
						result = result + '0';
					}
				}
				int resultint = Convert.ToInt32 (result, 2);
				RealiMasina.memory [zodis1] = resultint;
				break;
			case 10:
				Console.WriteLine("Vykdoma komanda INV");
				char[] bin = Convert.ToString (zodis1, 2).PadLeft (4, '0').ToCharArray ();
				string result = "";
				for (int i = 0; i < bin.Length; i++) {
					if (bin[i] == '0') 
					{
						result = result + '1';
					} else {
						result = result + '0';
					}
				}
				int resultint = Convert.ToInt32 (result, 2);
				RealiMasina.memory [zodis1] = resultint;
				break;
			case 11:
				Console.WriteLine("Vykdoma komanda JMP");
				break;
			case 12:
				Console.WriteLine("Vykdoma komanda JL");
				break;
			case 13:
				Console.WriteLine("Vykdoma komanda JE");
				break;
			case 14:
				Console.WriteLine("Vykdoma komanda JNE");
				break;
			case 15:
				Console.WriteLine("Vykdoma komanda JG");
				break;
			case 16:
				Console.WriteLine("Vykdoma komanda GD");
				break;
			case 17:
				Console.WriteLine("Vykdoma komanda PD");
				break;
			case 18:
				Console.WriteLine("Vykdoma komanda GDI");
				break;
			case 19:
				Console.WriteLine("Vykdoma komanda PDI");
				break;
			case 100:   //Show Registers
				Console.WriteLine("Vykdoma komanda Show Reg");
				string allRegisters =
					"R1:" + RealiMasina.R1 + " R2:" + RealiMasina.R2 + " PTR:" + RealiMasina.PTR + " SI:" + RealiMasina.SI + " PI:" + RealiMasina.PI +
                    " IOI:" + RealiMasina.IOI + " C:" + RealiMasina.C + " CH1:" + RealiMasina.CH1 + " CH2:" + RealiMasina.CH2 + " CH3:" + RealiMasina.CH3 +
                    " CH4:" + RealiMasina.CH4 + " MODE:" + RealiMasina.MODE + " TI:" + RealiMasina.TI + " CS:" + RealiMasina.CS + " DS:" + RealiMasina.DS +
                    " IC:" + RealiMasina.IC;
                    System.IO.File.WriteAllText(@"C:\Users\eivgai\Documents\Visual Studio 2015\Projects\OS\RegOut.txt", allRegisters); //Cia pasikeisk, kad pas tave butu
				break;
			case 101:   //Show Memory
				Console.WriteLine("Vykdoma komanda Show Memory");
				using (System.IO.StreamWriter file =
					new System.IO.StreamWriter(@"C:\Users\eivgai\Documents\Visual Studio 2015\Projects\OS\MemOut.txt")) //Cia pasikeisk, kad pas tave butu
				{
					foreach (int line in RealiMasina.memory)
					{
						file.WriteLine(line);
					}
				}
				break;
			default:
				Console.WriteLine("Įvesta netinkama komanda");
				break;
            }
            RealiMasina.TI--;
        }
    }

    class RealiMasina
    {
        public static int R1, R2, PTR;
        public static byte SI, PI, IOI, C, CH1, CH2, CH3, CH4, MODE, TI = 9;
        public static short CS, DS, IC;
        public static string RMKomanda, CMDKomanda, FailoVardas, allRegisters;
        public static int[] memory = new int[65536];

        public static void KomandosPatikrinimas()      //Komandos skaitymas ir tikrinimas
        {
            var Zodziai = RMKomanda.Split(' ');

            if (Zodziai[0] == "STARTC")
            {
                VirtualiMasina.CMDRezimas();
            }
            else if (Zodziai[0] == "STARTV")     //STARTV paskutini kart neveike dar del kazko, bet kolkas nelabai reiks
            {
                FailoVardas = Zodziai[1];
                VirtualiMasina.VRezimas();
            }
            else
            {
                if (Zodziai.Length > 2)
                {
                    Console.WriteLine("Ivesta komanda:" + Zodziai[0]);
                    RealiMasina.IC++;
                    int numVal1 = Int32.Parse(Zodziai[0]);
                    int numVal2 = Int32.Parse(Zodziai[1]);
                    int numVal3 = Int32.Parse(Zodziai[2]);
                    Commands.a = numVal1;
                    Commands.Vykdom(numVal2, numVal3);
                }
                else
                {
                    Console.WriteLine("Įvesta per mažai argumentų");
                    RealiMasina.IC++;
                    RealiMasina.PI = 1;
                    RealiMasina.PertraukimuTikrinimas();
                }
            }

        }

        public static void PertraukimuTikrinimas()          //Po visu pertraukimu pereinama i Realia masina (MODE = 0) ir atstatomi pertraukimu registrai
        {
            if (IOI > 0 | TI == 0 | SI > 0 | PI > 0)
            {
                Console.WriteLine("Vyksta pertraukimas");
                if (IOI > 0)
                {
                    Console.WriteLine("IOI > 0");
                    RealiMasina.MODE = 0;
                    RealiMasina.IOI = 0;
                }
                if (TI == 0)
                {
                    Console.WriteLine("TI = 0");
                    RealiMasina.MODE = 0;
                    RealiMasina.TI = 0;
                }
                if (SI > 0)
                {
                    Console.WriteLine("SI > 0");
                    RealiMasina.MODE = 0;
                    RealiMasina.SI = 0;

                }
                if (PI > 0)
                {
                    Console.WriteLine("PI > 0");
                    RealiMasina.MODE = 0;
                    RealiMasina.PI = 0;
                }
            }
        }
        public static void Puslapiavimas()      // Puslapiavimas kolkas neveikia, nezinau kaip daryt
        {
            int min = RealiMasina.PTR;
            RealiMasina.CS = 25;
            int max = RealiMasina.PTR + (128 * 4 + RealiMasina.CS); //Vienas blokas yra 128 zodziai. 
            RealiMasina.PTR = max;
            for (int i = min; i < max; i++)
            {
                if (i > RealiMasina.CS )
                {
                    RealiMasina.memory[i] = i - RealiMasina.CS;
                }
                else
                {
                    RealiMasina.memory[i] = 1;
                }
            }         
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Pradedama reali mašina");
            while ((RMKomanda = Console.ReadLine()) != "EXIT")                //Cia visada veikia Reali masina iki EXIT (mano sugalvota komanda)
            {
                MODE = 0;
                PTR = IC + CS;
                PertraukimuTikrinimas();
                Puslapiavimas();
                KomandosPatikrinimas();
                PertraukimuTikrinimas();
                TI--;
                PertraukimuTikrinimas();
                IC++;
            }
            Console.WriteLine("Reali mašina išjungiama");
            RMKomanda = Console.ReadLine();

        }
    }
}

