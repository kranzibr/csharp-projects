using System;
using System.Timers;

namespace Stopwatch
{
    class Program
    {
        static bool isPaused = false;
        static System.Timers.Timer timer = new System.Timers.Timer(1000); // Atualizar o cronômetro a cada 1 segundo
        static int remainingTime = 0;
        static int totalTime = 0; // Tempo total da contagem regressiva
        static object lockObject = new object(); // Variável de bloqueio para evitar execução concorrente

        // Função principal que inicia o programa
        static void Main(string[] args)
        {
            Console.WriteLine("Bem-vindo ao Stopwatch!");
            System.Threading.Thread.Sleep(2000); // Aguarda 2 segundos antes de exibir o menu
            Menu();
        }

        // Função para exibir o menu principal
        static void Menu()
        {
            Console.Clear();

            Console.WriteLine("S = Segundo => 10s = 10 segundos");
            Console.WriteLine("M = Minuto => 1m = 1 minuto");
            Console.WriteLine("0 = Sair");
            Console.WriteLine("Quanto tempo deseja contar?");

            string data = (Console.ReadLine() ?? "").ToLower();

            // Verificar se a entrada é '0' para sair do programa
            if (data == "0")
            {
                System.Environment.Exit(0);
            }

            char type = char.Parse(data.Substring(data.Length - 1, 1));

            // Tenta converter a parte numérica da entrada em um número inteiro
            if (!int.TryParse(data.Substring(0, data.Length - 1), out int time))
            {
                Console.WriteLine("Entrada inválida. Tente novamente.");
                System.Threading.Thread.Sleep(2000);
                Menu();
                return;
            }

            int multiplier = 1;

            if (type == 'm')
                multiplier = 60;

            totalTime = time * multiplier;
            PreStart(totalTime);
        }

        // Função que prepara o cronômetro para iniciar a contagem regressiva

        static void PreStart(int time)
        {
            Console.Clear();
            Console.WriteLine("Preparar...");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Apontar...");
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("Sua contagem regressiva começou!");
            System.Threading.Thread.Sleep(2500);

            Start(time);
        }

        // Função que realiza a contagem regressiva

        static void Start(int time)
        {
            remainingTime = time;

            // Configurar o timer para atualizar o cronômetro a cada segundo
            timer.Elapsed += UpdateTimer;
            timer.Start();

            while (remainingTime > 0)
            {
                if (!isPaused)
                {
                    // Mostra a contagem regressiva em tempo real
                    Console.Clear();
                    Console.WriteLine("Pressione 'P' para pausar");
                    Console.WriteLine("");
                    Console.WriteLine($"Tempo restante: {remainingTime} segundos.");
                }

                // Aguarda 1 segundo antes de atualizar o cronômetro novamente
                System.Threading.Thread.Sleep(1000);

                // Verifica se o usuário quer pausar o cronômetro
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.P)
                    {
                        isPaused = !isPaused; // Inverte o estado da pausa
                        if (isPaused)
                        {
                            Console.WriteLine("\nCronômetro pausado. Pressione 'P' para retomar...");
                        }
                        else
                        {
                            Console.WriteLine("\nRetomando a contagem regressiva...");
                        }
                    }
                }
            }

            // Parar o timer e a execução do cronômetro
            timer.Stop();

            Console.Clear();
            Console.WriteLine("Stopwatch finalizado!");
            System.Threading.Thread.Sleep(2500);
            Menu();
        }

        // Função que atualiza o cronômetro a cada segundo

        static void UpdateTimer(object sender, ElapsedEventArgs e)
        {
            lock (lockObject) // Bloquear a execução concorrente do código
            {
                if (!isPaused && remainingTime > 0)
                {
                    remainingTime--;
                }
            }
        }
    }
}
