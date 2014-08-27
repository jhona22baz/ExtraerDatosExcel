using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Data.Entity;
using System.Data.EntityModel;

namespace ExtraerDatosExcel
{
    class Program
    {
        public class Tiempo
        {
            public int Horas { get; set; }
            public int Minutos { get; set; }
            public int Segundos { get; set; }

            public override string ToString()
            {
                return string.Format("{0}:{1}:{2}",Horas, Minutos, Segundos);
            }

            public Tiempo SumarTiempo(int hr, int min, int sec)
            {
                Tiempo tmp = new Tiempo();
                return tmp;
            }
            public Tiempo ConvertTo(string tiempo) 
            {                
                Tiempo tmp = new Tiempo();
                string[] numbers = tiempo.Split(':');
                tmp.Horas = Convert.ToInt16(numbers[0]);
                tmp.Minutos = Convert.ToInt16(numbers[1]);
                tmp.Segundos = Convert.ToInt16(numbers[2]);                
                return tmp;
            }
        }
        public class Registro
        {
            public string Nomina { get; set; }
            public string Maquina { get; set; }
            public DateTime FechaInicial { get; set; }
            public DateTime FechaFinal { get; set; }
            public Tiempo tiempo {get; set;}
            public String tipoProceso { get; set; }
            public String Proceso { get; set; }
            public string turno { get; set; }
            public override string ToString()
            {
                return string.Format("[{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}]", Nomina,Maquina,FechaInicial,FechaFinal,tiempo,tipoProceso,Proceso, turno);
            }
        }
        public class Excel 
        {
            public void crear_excel(List<Registro> registros, string NExcel) 
            {
                IWorkbook wordBook = new HSSFWorkbook();
                ISheet sheet = wordBook.CreateSheet("Datos");
                IRow headerRow = sheet.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("Nomina");
                headerRow.CreateCell(1).SetCellValue("Máquina");
                headerRow.CreateCell(2).SetCellValue("Fecha Inicial");
                headerRow.CreateCell(3).SetCellValue("fecha Final");
                headerRow.CreateCell(4).SetCellValue("Tiempo");
                headerRow.CreateCell(5).SetCellValue("Tipo proceso");
                headerRow.CreateCell(6).SetCellValue("Proceso");
                headerRow.CreateCell(7).SetCellValue("Turno");                
                int row = 1;
                foreach (var reg in registros) {
                    GenerateRow(sheet, row, reg);
                    row++;
                }
                FileStream fs = File.Create(@"C:\Users\JAR\Desktop\Excel_ Pruebas\SALIDA\" + NExcel + ".xls");
                wordBook.Write(fs);
                fs.Close();
                
            }

            public void GenerateRow(ISheet sheet, int rowid, Registro reg)
            {
                IRow row = sheet.CreateRow(rowid);
                row.CreateCell(0).SetCellValue(reg.Nomina);
                row.CreateCell(1).SetCellValue(reg.Maquina);
                row.CreateCell(2).SetCellValue(reg.FechaInicial.ToString());
                row.CreateCell(3).SetCellValue(reg.FechaFinal.ToString());
                row.CreateCell(4).SetCellValue(reg.tiempo.ToString());
                row.CreateCell(5).SetCellValue(reg.tipoProceso);
                row.CreateCell(6).SetCellValue(reg.Proceso);
                row.CreateCell(7).SetCellValue(reg.turno);
            }
        }        
        public class ObtenerDatos 
        {
            Dictionary<string, string> colaboradores = new Dictionary<string, string>()
            {
                {"0","SUPERVISOR"},{"1","MANTENIMIENTO"},{"2","Sin Llave"},{"1862", "123456789"},{"1863", "000001"},{"1864", "000002"},{"1865", "000003"},{"1866", "000005"},
                {"1867", "000004"},{"1868", "000006"}
            };

             Dictionary<string, string> maquinas = new Dictionary<string, string>()
            {
                 {"737","MC 10109"},{"739","00001"},{"740","00002"}
            };

             Dictionary<string, string> procesos = new Dictionary<string, string>()
             {
                {"12641","DESVIACION DE PROCESO:Quitar  Oxido"},{"12642","DESVIACION DE PROCESO:Qitar un excedente"},{"12643","DESVIACION DE PROCESO:Gaps"},
                {"12644","DESVIACION DE PROCESO:Ajuste (Componente y/o Dimension)"},{"12645","DESVIACION DE PROCESO:Soldadura arco aire INACTIVO"},
                {"12646","DESVIACION DE PROCESO:Pulido Estetico"},{"12647","DESVIACION DE PROCESO:Moviendo Material"},
                {"12648","DESVIACION DE PROCESO:Stop to Fix "},{"12897","FALTA DE:Material (Componentes)"},
                {"12898","FALTA DE:Herramienta (Incluso Soldadura)"},{"12899","FALTA DE:Ayuda  Visual/ Procedimiento)"},
                {"13153","ESPERA DE:Inspector"},{"13154","ESPERA DE:Material"},
                {"13155","ESPERA DE:Laboratorio (USL)"},{"13156","ESPERA DE:Trazo"},
                {"13157","ESPERA DE:Montacargas"},{"13158","ESPERA DE:Grua"},
                {"13409","FALLA DE:Grua"},{"13410","FALLA DE:Aditamento/Posicionador"},
                {"13411","FALLA DE:Maquina de Soldar"},{"13412","FALLA DE:Pistola"},
                {"13665","ASISTENCIA A:Junta"},{"13666","ASISTENCIA A:Almacen"},
                {"13667","ASISTENCIA A:Depto Medico"},{"13668","ASISTENCIA A:Sindicato"},
                {"13921","MANTENIMIENTO:Autonomo"},{"13922","MANTENIMIENTO:Preventivo "},
                {"29537","DESVIACION DE PROCESO:Soldadura arco aire "},{"29806","TRABAJO:Proceso Normal"},
                {"14177","ENSAMBLE:Inicio Proceso.. "},{"4178 ","ENSAMBLE:Termino Proceso "},
                {"14433","TIEMPO PERSONAL:Ir al baño "},{"14434","TIEMPO PERSONAL:Ir al comedor "},
                {"29548","SIN LLAVE OPERADOR:tiempo sin llave "},{"24933","AHORRO:ahorro de energia "}
             };

             public string GetValueColaboradores(string llave)
             {
                 if (colaboradores.ContainsKey(llave))
                     return colaboradores[llave];
                 return  "error"; // Example only
             }
             public string GetValueMaquinas(string llave) 
             {
                 if (maquinas.ContainsKey(llave))
                     return maquinas[llave];
                 return "error";
             }
             public string getValueProceso(string llave)
             {
                 string[] tiposProcesos;
                 if (procesos.ContainsKey(llave))
                 {
                     tiposProcesos = procesos[llave].Split(':');
                     return tiposProcesos[1];
                 }
                 return "error";
             }
             public string getValueTipoProceso(string llave)
             {
                 string[] tiposProcesos;
                 if (procesos.ContainsKey(llave))
                 {
                     tiposProcesos = procesos[llave].Split(':');
                     return tiposProcesos[0];
                 }
                 return "error";                 
             }
             public Registro ConvertirRegistro(Registro reg) 
             {
                 Registro Nuevo = new Registro();
                 Nuevo.Nomina = GetValueColaboradores(reg.Nomina);
                 Nuevo.Maquina = GetValueMaquinas(reg.Maquina);
                 Nuevo.FechaInicial = reg.FechaInicial;
                 Nuevo.FechaFinal = reg.FechaFinal;
                 Nuevo.tiempo = reg.tiempo;
                 Nuevo.tipoProceso = getValueTipoProceso(reg.tipoProceso);
                 Nuevo.Proceso = getValueProceso(reg.Proceso);                 
                 return Nuevo;
             }
        }
        static void Main(string[] args)
        {
            HSSFWorkbook wordbook;
            List<Registro> registros = new List<Registro>();
            List<Registro> registrosN = new List<Registro>(); 
            Tiempo Ayuda = new Tiempo();
            Excel crear = new Excel();
            ObtenerDatos regBien = new ObtenerDatos();
            CAEMPRUEBASEntities Resultados = new CAEMPRUEBASEntities();

            Resultados.Tipo_proceso.FirstOrDefault(a => a.descripcion == "a");
            Tipo_proceso tipo = Resultados.Tipo_proceso.FirstOrDefault(a => a.descripcion == "29548");

            //Console.WriteLine(" descripcion {0}", tipo.descripcion);
            Maquinas maq = Resultados.Maquinas.FirstOrDefault(a => a.ID == 739);
            Console.WriteLine("la maquina es {0}", maq.Numero);
            //739	07/06/2014 11:07:26	07/06/2014 11:07:26	00:00:00	29548
            
            List<int> numeros = new List<int>();
            using (FileStream file = new FileStream(@"C:\Users\JAR\Desktop\Excel_ Pruebas\739-2014-06-18-14-44-35.xls", FileMode.Open, FileAccess.Read))
            {
                wordbook = new HSSFWorkbook(file);
            }
            
            ISheet sheet = wordbook.GetSheet("Datos");
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                Registro registro = new Registro();
                
                if (sheet.GetRow(row) != null)
                {                   
                    registro.Nomina = sheet.GetRow(row).GetCell(0).StringCellValue;
                    registro.Maquina = sheet.GetRow(row).GetCell(1).StringCellValue;
                    registro.FechaInicial = Convert.ToDateTime( sheet.GetRow(row).GetCell(2).StringCellValue);
                    registro.FechaFinal = Convert.ToDateTime(sheet.GetRow(row).GetCell(3).StringCellValue);
                    registro.tiempo = Ayuda.ConvertTo(sheet.GetRow(row).GetCell(4).StringCellValue);
                    registro.tipoProceso = sheet.GetRow(row).GetCell(5).StringCellValue;
                    registro.Proceso = sheet.GetRow(row).GetCell(5).StringCellValue;
                    registro.turno = "";                    
                    registros.Add(regBien.ConvertirRegistro(registro));
                }
            }

            foreach (var reg in registros)
            {

                if (reg.Proceso.Contains("Proceso Normal"))
                    registrosN.Add(reg);
            }
            crear.crear_excel(registrosN, "Ma00001TNormal");
            crear.crear_excel(registros,"Maquina00001");
            Console.WriteLine("termino");
            Console.ReadLine();
        }
    }
}
