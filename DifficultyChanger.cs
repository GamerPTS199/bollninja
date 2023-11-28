using UnityEngine;

public class DifficultyChanger : MonoBehaviour
{
    // Шаг в количестве очков, при котором увеличивается сложность
    public int DifficultyUpScoreStep = 30;

    // Максимальная сложность игры
    public int MaxDifficult = 20;

    // Текущая сложность игры
    private int _difficult = 1;

    // Очки, при которых последний раз увеличивалась сложность
    private int _lastDifficultyUpScore = 0;

    public float CalculateRandomSpawnDelay(float minDelay, float maxDelay)
    {
        // Получаем случайную задержку в появлении объектов
        float randomDelay = Random.Range(minDelay, maxDelay);

        // Вводим коэффициент сложности, который будет принимать значения от 0 до 1 и становиться меньше с увеличением сложности
        float difficultyCoef = (float)(MaxDifficult - _difficult) / MaxDifficult;

        // Получаем разницу между случайным и минимальным значением задержки появления объектов
        float delayDelta = randomDelay - minDelay;

        // Разницу умножаем на коэффициент сложности — так объекты будут появляться чаще параллельно с увеличением сложности
        return minDelay + delayDelta * difficultyCoef;
    }
    public float CalculateBombChance(float minChance, float maxChance)
    {
        // Вводим коэффициент сложности, который будет принимать значения от 0 до 1 и становиться меньше с увеличением сложности
        float difficultyCoef = (float)_difficult / MaxDifficult;

        // Получаем разницу между максимальным и минимальным значением шанса появления бомбы
        float chanceDelta = maxChance - minChance;

        // Разницу умножаем на коэффициент сложности — так бомбы будут появляться чаще параллельно с увеличением сложности
        return minChance + chanceDelta * difficultyCoef;
    }
    public void Restart()
    {
        // Делаем текущую сложность минимальной
        _difficult = 1;

        // Сбрасываем (приравниваем к нулю) количество очков, при которых увеличивалась сложность
        _lastDifficultyUpScore = 0;
    }
    public void SetDifficultByScore(int score)
    {
        // Проверяем, что текущее количество очков больше, чем сумма шага увеличения и того количества очков, на котором мы последний раз увеличивали сложность; также проверяем, что текущая сложность — ещё не максимальная
        if (score > _lastDifficultyUpScore + DifficultyUpScoreStep && _difficult < MaxDifficult)
        {
            // Складываем количество очков, при котором мы в последний раз увеличили сложность, с шагом увеличения сложности
            _lastDifficultyUpScore += DifficultyUpScoreStep;

            // Увеличиваем текущую сложность на единицу
            _difficult++;
        }
    }

}
