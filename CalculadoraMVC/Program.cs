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

        Console.Title = $"Calculadora - Aula MVC - ACELERA .NET";
    }

    private static void ShowMenu()
    {
        bool sair = false;

        do
        {
            AnsiConsole.Write(
                new FigletText("CALCULADORA!")
                    .LeftJustified()
                    .Color(Color.LightCyan3));
            AnsiConsole.WriteLine("");

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
        string nome = AnsiConsole.Ask<string>("Digite o nome: ");

        double peso = AnsiConsole.Prompt(new TextPrompt<double>("Digite o peso (kg): ")
                                    .ValidationErrorMessage("[red]O valor digitado não é um valor válido para peso.[/]")
                                    .Validate(p =>
                                    {
                                        return p switch
                                        {
                                            <= 0.0d => ValidationResult.Error("[red]Não é possível ter 0,0 kg de peso.[/]"),
                                            >= 500.0d => ValidationResult.Error("[red]Não é possível ter mais de 500,00 kg de peso (valor máximo para peso).[/]"),
                                            _ => ValidationResult.Success(),
                                        };
                                    }));

        int altura = AnsiConsole.Prompt(new TextPrompt<int>("Digite a altura (cm): ")
                                    .ValidationErrorMessage("[red]O valor digitado não é um valor válido para altura.[/]")
                                    .Validate(a =>
                                    {
                                        return a switch
                                        {
                                            <= 0 => ValidationResult.Error("[red]Não é possível ter 0 centímetros de altura.[/]"),
                                            >= 300 => ValidationResult.Error("[red]É preciso ter menos de 300 centímetros de altura (valor máximo para altura).[/]"),
                                            _ => ValidationResult.Success(),
                                        };
                                    }));

        int idade = AnsiConsole.Prompt(new TextPrompt<int>("Digite a idade: ")
                                    .ValidationErrorMessage("[red]O valor digitado não é um valor válido para idade.[/]")
                                    .Validate(i =>
                                    {
                                        return i switch
                                        {
                                            <= 0 => ValidationResult.Error("[red]É preciso ter ao menos 1 ano de idade.[/]"),
                                            >= 150 => ValidationResult.Error("[red]É preciso ter menos de 150 anos de idade (valor máximo para idade).[/]"),
                                            _ => ValidationResult.Success(),
                                        };
                                    }));

        double imc = Math.Round(peso / Math.Sqrt(altura / 100), 2);

        AnsiConsole.WriteLine("");
        AnsiConsole.WriteLine($"O IMC de {nome} é: {imc:0.00}");

        AnsiConsole.WriteLine("Classificação: " + ClassificarIMC(imc));
        
        ApertarContinuar();
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
        string[] posicaoNotas = new string[] { "primeira", "segunda", "terceira" };
        double[] notas = new double[3];

        for (int i = 0; i < posicaoNotas.Length; i++)
        {
            notas[i] = ValidarNotaInserida(posicaoNotas[i]);
        }

        double media = (notas[0] + notas[1] + notas[2]) / 3;

        AnsiConsole.WriteLine("O aluno foi " + VerificarAprovacao(media));

        AnsiConsole.WriteLine($"Média: {media:#.00}");

        ApertarContinuar();
    }

    private static double ValidarNotaInserida(string posicao)
    {
        // TODO: Criar instância padronizada no InitializeComponents
        const double minValue = 0.0d;
        const double maxValue = 10.0d;

        return AnsiConsole.Prompt(new TextPrompt<double>($"Digite a {posicao} nota: ")
                                .ValidationErrorMessage("[red]O valor digitado não é válido para nota.[/]")
                                .Validate(n =>
                                {
                                    return n switch
                                    {
                                        < minValue => ValidationResult.Error($"[red]O valor mínimo da nota é {minValue:0.00}.[/]"),
                                        > maxValue => ValidationResult.Error($"[red]O valor máximo da nota é {maxValue:0.00}.[/]"),
                                        _ => ValidationResult.Success(),
                                    };
                                }));
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

    private static void ApertarContinuar()
    {
        AnsiConsole.WriteLine("Aperte alguma tecla para continuar.");
        Console.ReadKey();
        AnsiConsole.Clear();
    }
}