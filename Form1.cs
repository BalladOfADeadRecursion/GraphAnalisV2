namespace GraphAnalisV2
{
    // Определяем класс формы приложения
    public partial class Form1 : Form
    {
        // Конструктор формы, вызывающий инициализацию компонентов
        public Form1()
        {
            InitializeComponent(); // Инициализирует компоненты формы (создаёт элементы интерфейса)
        }

        private int[,] adjacencyMatrix; // Объявляем переменную для хранения матрицы смежности графа
        private int n; // Число вершин графа
        private List<int> setA = new List<int>(); // Список для хранения вершин первой доли (для двудольного графа)
        private List<int> setB = new List<int>(); // Список для хранения вершин второй доли (для двудольного графа)

        // Обработчик нажатия кнопки 1 (для загрузки данных из файла)
        private void button1_Click(object sender, EventArgs e)
        {
            // Считываем путь к файлу из текстового поля richTextBox1
            string filePath = richTextBox1.Text.Trim();

            // Проверяем, существует ли файл по указанному пути
            if (File.Exists(filePath))
            {
                try
                {
                    // Читаем содержимое файла и выводим его в richTextBox2
                    string fileContent = File.ReadAllText(filePath);
                    richTextBox2.Text = fileContent; // Записываем содержимое файла в richTextBox2
                }
                catch (Exception ex)
                {
                    // Если произошла ошибка при чтении файла, выводим сообщение об ошибке
                    MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Если файл не найден, выводим предупреждение
                MessageBox.Show("The specified file was not found. Check the path and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Обработчик нажатия кнопки 2 (для анализа графа)
        private void button2_Click(object sender, EventArgs e)
        {
            // Разделяем текст, полученный из richTextBox2, на строки по символу новой строки
            string[] lines = richTextBox2.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            // Считываем количество вершин из первой строки
            n = int.Parse(lines[0].Trim());

            // Инициализируем матрицу смежности размером n x n
            adjacencyMatrix = new int[n, n];

            // Заполняем матрицу смежности
            for (int i = 0; i < n; i++) // Проходим по всем строкам матрицы
            {
                // Разделяем строку на отдельные элементы, которые представляют собой значения в строке матрицы
                string[] row = lines[i + 1].Trim().Split(' ');

                for (int j = 0; j < n; j++) // Проходим по каждому элементу строки
                {
                    adjacencyMatrix[i, j] = int.Parse(row[j]); // Заполняем матрицу смежности значениями из строки
                }
            }

            // Массив для хранения цветов вершин графа
            bool[] color = new bool[n];
            // Массив для отслеживания, были ли вершины посещены
            bool[] visited = new bool[n];
            // Флаг для проверки, является ли граф двудольным
            bool isBipartite = true;

            // Очищаем списки вершин для обеих долей
            setA.Clear();
            setB.Clear();

            // Используем очередь для реализации BFS
            Queue<int> queue = new Queue<int>();

            // Проходим по всем вершинам графа
            for (int i = 0; i < n; i++)
            {
                if (!visited[i]) // Если вершина еще не была посещена
                {
                    // Добавляем вершину в очередь для обхода
                    queue.Enqueue(i);

                    // Начинаем с первого цвета для текущей вершины
                    color[i] = true;
                    visited[i] = true; // Помечаем вершину как посещенную
                    setA.Add(i + 1); // Добавляем вершину в первую долю (нумерация с 1)

                    // BFS-обход графа
                    while (queue.Count > 0) // Пока в очереди есть элементы
                    {
                        int vertex = queue.Dequeue(); // Извлекаем вершину из очереди

                        // Проходим по всем смежным вершинам текущей
                        for (int j = 0; j < n; j++)
                        {
                            if (adjacencyMatrix[vertex, j] == 1) // Если есть ребро между вершинами vertex и j
                            {
                                if (!visited[j]) // Если вершина j еще не посещена
                                {
                                    visited[j] = true; // Помечаем вершину как посещенную
                                    color[j] = !color[vertex]; // Меняем цвет вершины
                                    queue.Enqueue(j); // Добавляем вершину в очередь

                                    // В зависимости от цвета добавляем вершину в одну из долей
                                    if (color[j]) setA.Add(j + 1); // Если цвет true, добавляем в setA (вторую долю)
                                    else setB.Add(j + 1); // Если цвет false, добавляем в setB (первую долю)
                                }
                                else if (color[j] == color[vertex]) // Если смежные вершины одного цвета, граф не двудольный
                                {
                                    isBipartite = false; // Граф не двудольный
                                }
                            }
                        }
                    }
                }
            }

            // Добавляем пробел в richTextBox2 для отступа
            richTextBox2.Text += "\n\n";

            // Печатаем все вершины графа (нумерация с 1)
            for (int i = 0; i < n; i++)
            {
                richTextBox2.Text += (i + 1) + " "; // Добавляем номер вершины в текст
            }

            richTextBox2.Text += "\n\n"; // Добавляем новую строку

            // Печатаем вершины первой и второй доли в графе
            if (isBipartite) // Если граф двудольный
            {
                richTextBox2.Text += "Граф двудольный\n\n"; // Пишем, что граф двудольный
                richTextBox2.Text += "Вершины первой доли: " + string.Join(" ", setA) + "\n"; // Выводим вершины первой доли
                richTextBox2.Text += "Вершины второй доли: " + string.Join(" ", setB) + "\n"; // Выводим вершины второй доли
                richTextBox2.Text += "\n";

                // Ищем паросочетание (алгоритм для поиска паросочетаний)
                int[] match = new int[n];
                Array.Fill(match, -1); // Заполняем массив паросочетаний значением -1, что означает отсутствие пары

                // Простейший алгоритм поиска паросочетания
                foreach (int vertex in setA)
                {
                    bool[] visitedMatch = new bool[n]; // Массив для отслеживания посещенных вершин при поиске пары
                    if (FindMatch(vertex - 1, match, visitedMatch)) // Ищем паросочетание для вершины
                    {
                        richTextBox2.Text += (vertex) + " "; // Если нашли пару, выводим вершину
                    }
                }

                richTextBox2.Text += "\n"; // Добавляем новую строку для паросочетания
                richTextBox2.Text += "\nПаросочетание:\n"; // Заголовок для паросочетания
                richTextBox2.Text += "\n";

                // Печатаем пары, которые образуют паросочетание
                for (int i = 0; i < n; i++)
                {
                    if (match[i] != -1) // Если для вершины есть пара
                    {
                        richTextBox2.Text += (i + 1) + " - " + (match[i] + 1) + "\n"; // Печатаем пару
                    }
                }
            }
            else
            {
                richTextBox2.Text += "Граф не двудольный\n"; // Если граф не двудольный, выводим соответствующее сообщение
            }
        }

        // Метод для поиска паросочетания
        private bool FindMatch(int u, int[] match, bool[] visited)
        {
            // Проходим по всем вершинам для поиска возможной пары для вершины u
            for (int v = 0; v < n; v++)
            {
                if (adjacencyMatrix[u, v] == 1 && !visited[v]) // Если существует ребро и вершина v не посещена
                {
                    visited[v] = true; // Помечаем вершину как посещенную

                    // Если вершина v не имеет пары или если мы нашли альтернативную пару для вершины v
                    if (match[v] == -1 || FindMatch(match[v], match, visited))
                    {
                        match[v] = u; // Устанавливаем пару для вершины v
                        return true; // Возвращаем, что пара найдена
                    }
                }
            }
            return false; // Возвращаем false, если пара не найдена
        }

        // Обработчик нажатия кнопки 3 (для очистки текстовых полей)
        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = ""; // Очищаем текст в richTextBox1
            richTextBox2.Text = ""; // Очищаем текст в richTextBox2
        }
    }
}
