using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Estudiante
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public int Edad { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}, Nombre: {Nombre}, Edad: {Edad}";
    }
}

public class Materia
{
    public string Nombre { get; set; }
    public decimal Costo { get; set; }

    public override string ToString()
    {
        return $"{Nombre} - Costo: {Costo} colones";
    }
}

public class Matricula
{
    public Estudiante Estudiante { get; set; }
    public List<Materia> Materias { get; set; }

    public Matricula()
    {
        Materias = new List<Materia>();
    }

    public decimal CalcularCostoTotal()
    {
        decimal costoMaterias = Materias.Sum(m => m.Costo);
        decimal costoMatricula = 63400; // Costo fijo de matrícula
        return costoMaterias + costoMatricula;
    }

    public override string ToString()
    {
        var matriculaInfo = $"Resumen de Matrícula para {Estudiante.Nombre} (ID: {Estudiante.Id}, Edad: {Estudiante.Edad}):\n";
        matriculaInfo += "Materias matriculadas:\n";
        foreach (var materia in Materias)
        {
            matriculaInfo += $"- {materia}\n";
        }
        matriculaInfo += $"Costo total de materias: {Materias.Sum(m => m.Costo)} colones\n";
        matriculaInfo += $"Costo de matrícula: 63400 colones\n";
        matriculaInfo += $"Costo total: {CalcularCostoTotal()} colones";
        return matriculaInfo;
    }
}

public enum MenuOption
{
    RegistrarEstudiante = 1,
    EditarNombreEstudiante,
    VerMatriculas,
    SeleccionarMaterias,
    MostrarResumen,
    GuardarMatricula,
    Salir
}

class Program
{
    private static List<Matricula> matriculas = new List<Matricula>();
    private static List<Materia> materiasDisponibles = new List<Materia>
    {
        new Materia { Nombre = "Matemáticas", Costo = 76200 },
        new Materia { Nombre = "Física", Costo = 76200 },
        new Materia { Nombre = "Programación", Costo = 76200 },
        new Materia { Nombre = "Química", Costo = 76200 },
        new Materia { Nombre = "Biología", Costo = 76200 },
        new Materia { Nombre = "Literatura", Costo = 76200 }
    };

    private static bool estudianteRegistrado = false;

    static void Main(string[] args)
    {
        Console.WriteLine("Bienvenido a la aplicación de matrícula universitaria\n");

        bool continuar = true;
        while (continuar)
        {
            MostrarMenu();
            MenuOption opcion;
            while (!Enum.TryParse(Console.ReadLine(), out opcion) || !Enum.IsDefined(typeof(MenuOption), opcion))
            {
                Console.WriteLine("Opción inválida. Por favor, elige una opción del menú.");
            }

            switch (opcion)
            {
                case MenuOption.RegistrarEstudiante:
                    RegistrarEstudiante();
                    break;
                case MenuOption.EditarNombreEstudiante:
                    EditarNombreEstudiante();
                    break;
                case MenuOption.VerMatriculas:
                    VerMatriculas();
                    break;
                case MenuOption.SeleccionarMaterias:
                    SeleccionarMaterias();
                    break;
                case MenuOption.MostrarResumen:
                    MostrarResumen();
                    break;
                case MenuOption.GuardarMatricula:
                    GuardarMatricula();
                    break;
                case MenuOption.Salir:
                    continuar = false;
                    break;
            }

            Console.WriteLine();
        }
    }

    static void MostrarMenu()
    {
        Console.WriteLine("Menú:");
        Console.WriteLine("1. Registrar Estudiante");
        Console.WriteLine("2. Editar Nombre del Estudiante");
        Console.WriteLine("3. Ver Matrículas Registradas");
        Console.WriteLine("4. Seleccionar Materias");
        Console.WriteLine("5. Mostrar Resumen de Matrícula");
        Console.WriteLine("6. Guardar Matrícula en Archivo");
        Console.WriteLine("7. Salir");
        Console.Write("Elige una opción: ");
    }

    static void RegistrarEstudiante()
    {
        Console.WriteLine("\nRegistro de Estudiante:");
        Console.Write("ID: ");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id) || id <= 0)
        {
            Console.WriteLine("ID inválido. Ingresa un número entero positivo.");
            Console.Write("ID: ");
        }

        Console.Write("Nombre: ");
        string nombre = Console.ReadLine();

        Console.Write("Edad: ");
        int edad;
        while (!int.TryParse(Console.ReadLine(), out edad) || edad <= 0)
        {
            Console.WriteLine("Edad inválida. Ingresa un número entero positivo.");
            Console.Write("Edad: ");
        }

        var nuevaMatricula = new Matricula
        {
            Estudiante = new Estudiante { Id = id, Nombre = nombre, Edad = edad }
        };

        matriculas.Add(nuevaMatricula);
        estudianteRegistrado = true;
        Console.WriteLine("Estudiante registrado correctamente.");
    }

    static void EditarNombreEstudiante()
    {
        if (!estudianteRegistrado)
        {
            Console.WriteLine("Primero debes registrar un estudiante.");
            return;
        }

        Console.WriteLine("\nEditar Nombre del Estudiante:");
        Console.Write($"Nombre actual: {matriculas.Last().Estudiante.Nombre}. Ingresa el nuevo nombre: ");
        string nuevoNombre = Console.ReadLine();

        matriculas.Last().Estudiante.Nombre = nuevoNombre;
        Console.WriteLine("Nombre del estudiante actualizado correctamente.");
    }

    static void VerMatriculas()
    {
        if (matriculas.Count == 0)
        {
            Console.WriteLine("No hay matrículas registradas.");
            return;
        }

        Console.WriteLine("\nListado de Matrículas Registradas:");
        foreach (var matricula in matriculas)
        {
            Console.WriteLine($"ID: {matricula.Estudiante.Id}, Nombre: {matricula.Estudiante.Nombre}");
        }
    }

    static void SeleccionarMaterias()
    {
        if (!estudianteRegistrado)
        {
            Console.WriteLine("Primero debes registrar un estudiante.");
            return;
        }

        Console.WriteLine("\nSelecciona las materias:");

        int opcion;
        do
        {
            MostrarMateriasDisponibles();

            Console.WriteLine($"{materiasDisponibles.Count + 1}. Finalizar selección");
            Console.Write("Elige una materia (1-" + (materiasDisponibles.Count + 1) + "): ");
            while (!int.TryParse(Console.ReadLine(), out opcion) || opcion < 1 || opcion > materiasDisponibles.Count + 1)
            {
                Console.WriteLine("Opción inválida. Por favor, elige una opción válida.");
                Console.Write("Elige una materia (1-" + (materiasDisponibles.Count + 1) + "): ");
            }

            if (opcion <= materiasDisponibles.Count)
            {
                var materiaSeleccionada = materiasDisponibles[opcion - 1];
                if (!matriculas.Last().Materias.Contains(materiaSeleccionada))
                {
                    matriculas.Last().Materias.Add(materiaSeleccionada);
                    Console.WriteLine($"'{materiaSeleccionada.Nombre}' seleccionada correctamente.");
                }
                else
                {
                    Console.WriteLine($"La materia '{materiaSeleccionada.Nombre}' ya ha sido seleccionada.");
                }
            }
        } while (opcion != materiasDisponibles.Count + 1);

        Console.WriteLine("Selección de materias finalizada.");
    }

    static void MostrarMateriasDisponibles()
    {
        Console.WriteLine("Materias disponibles:");
        for (int i = 0; i < materiasDisponibles.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {materiasDisponibles[i]}");
        }
    }

    static void MostrarResumen()
    {
        if (!estudianteRegistrado)
        {
            Console.WriteLine("Primero debes registrar un estudiante.");
            return;
        }

        if (matriculas.Last().Materias.Count == 0)
        {
            Console.WriteLine("No se han seleccionado materias.");
            return;
        }

        Console.WriteLine("\nResumen de Matrícula:");
        Console.WriteLine(matriculas.Last());
    }

    static void GuardarMatricula()
    {
        if (!estudianteRegistrado)
        {
            Console.WriteLine("No hay información de matrícula para guardar.");
            return;
        }

        if (matriculas.Last().Materias.Count == 0)
        {
            Console.WriteLine("No se han seleccionado materias.");
            return;
        }

        string nombreArchivo = $"matricula_{matriculas.Last().Estudiante.Id}.txt";
        using (StreamWriter writer = new StreamWriter(nombreArchivo))
        {
            writer.WriteLine($"Resumen de Matrícula para {matriculas.Last().Estudiante.Nombre} (ID: {matriculas.Last().Estudiante.Id}, Edad: {matriculas.Last().Estudiante.Edad}):");
            writer.WriteLine("Materias matriculadas:");
            foreach (var materia in matriculas.Last().Materias)
            {
                writer.WriteLine($"- {materia}");
            }
            writer.WriteLine($"Costo total de materias: {matriculas.Last().Materias.Sum(m => m.Costo)} colones");
            writer.WriteLine($"Costo de matrícula: 63400 colones");
            writer.WriteLine($"Costo total: {matriculas.Last().CalcularCostoTotal()} colones");
        }

        Console.WriteLine($"\nMatrícula guardada en el archivo '{nombreArchivo}'.");
    }
}
