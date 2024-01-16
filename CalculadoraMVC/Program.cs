using Spectre.Console;
using Spectre.Console.Cli;

public class Program
{
    public static string TITLE { get; set; }
    public static string[] CHOICES { get; set; }

    private static void Main(string[] args)
    {
        // Inicializar todos os componentes necessários
        InitializeComponents();

        // Exibe o menu inicial e suas lógicas seguintes
        ShowMenu();

        // Finaliza a aplicação do Console App
        AnsiConsole.WriteLine("Concluímos a operação!");
        Console.ReadLine();
    }

    private static void InitializeComponents()
    {
        TITLE = "Escolha o tipo de cálculo que deseja fazer:";
        CHOICES = new string[] { "MÉDIA", "IMC", "SAIR" };
    }

    private static void ShowMenu()
    {
        AnsiConsole.Write(
            new FigletText("CALCULADORA!")
                .LeftJustified()
                .Color(Color.LightCyan3));
        AnsiConsole.WriteLine("");

        bool sair = false;

        do
        {
            var escolha = ShowSelectionPrompt(TITLE, CHOICES);

            switch (escolha)
            {
                case "MÉDIA":
                    CalcularMedia();
                    break;
                case "IMC":
                    CalcularIMC();
                    break;
                case "SAIR":
                    sair = true;
                    break;
            }
        } while (!sair);   
    }

    private static string ShowSelectionPrompt(string title, string[] choices)
    {
        return AnsiConsole.Prompt(new SelectionPrompt<string>()
                              .Title(title)
                              .AddChoices(choices));
    }

    private static void CalcularIMC()
    {
        AnsiConsole.Write("Digite o nome: ");
        string nome = Console.ReadLine();

        AnsiConsole.Write("Digite o peso (kg): ");
        double peso = Convert.ToDouble(Console.ReadLine());

        AnsiConsole.Write("Digite a altura (cm): ");
        double altura = Convert.ToDouble(Console.ReadLine());

        AnsiConsole.Write("Digite a idade: ");
        double idade = Convert.ToDouble(Console.ReadLine());

        double imc = Math.Round(peso / Math.Sqrt(altura / 100), 2);

        AnsiConsole.WriteLine("");
        AnsiConsole.WriteLine($"O IMC de {nome} é: {imc:#.00}");

        AnsiConsole.WriteLine("Classificação: " + ClassificarIMC(imc));
    }

    private static string ClassificarIMC(double imc)
    {
        return imc switch
        {
            < 18.5d => "Magreza",
            >= 18.5d and <= 24.9d => "Normal",
            >= 25.0d and <= 29.9d => "Sobrepeso",
            >= 30.0d and <= 39.9d => "Obesidade",
            >= 40.0d => "Obesidade grave",
            _ => "Valor não reconhecido"
        };
    }

    private static void CalcularMedia()
    {
        double[] notas = new double[3];
        AnsiConsole.WriteLine("Digite a primeira nota:");
        notas[0] = Convert.ToDouble(Console.ReadLine());

        AnsiConsole.WriteLine("Digite a segunda nota:");
        notas[1] = Convert.ToDouble(Console.ReadLine());

        AnsiConsole.WriteLine("Digite a terceira nota:");
        notas[2] = Convert.ToDouble(Console.ReadLine());

        double media = (notas[0] + notas[1] + notas[2]) / 3;

        AnsiConsole.WriteLine("O aluno foi " + VerificarAprovacao(media));

        AnsiConsole.WriteLine("Média: " + media);
    }

    private static string VerificarAprovacao(double media)
    {
        return media switch
        {
            < 5.0d => "Reprovado",
            >= 5.0d and < 7.0d => "Recuperação",
            >= 7.0d => "Aprovado",
            _ => "Valor não reconhecido"
        };
    }
}