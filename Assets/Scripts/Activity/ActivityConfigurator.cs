using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Activity
{
    public class ActivityConfigurator
    {
        #region API Methods

        public void Configure(ActivitySettings settings, ScreenTextNumber screenTextNumber, List<OptionBox> boxes)
        {
            int corretAnswer = GenerateAnswer(settings.MinRangeStatement, settings.MaxRangeStatement);
            screenTextNumber.Configure(Utils.NumberToString(corretAnswer));

            int correctBox = Random.Range(0, boxes.Count);

            List<int> answers = GenerateAnswers(settings, boxes.Count, corretAnswer, correctBox);

            ConfigureBoxes(boxes, answers, correctBox);
        }

        #endregion

        #region Other Methods

        private int GenerateAnswer(int min, int max)
        {
            return Random.Range(min, max + 1);
        }

        private List<int> GenerateAnswers(ActivitySettings settings, int numBoxes, int answer, int correctBox)
        {
            List<int> possibleAnswers = new List<int>();

            while (possibleAnswers.Count < numBoxes - 1)
            {
                int wrongAnswer = GenerateAnswer(settings.MinRangeStatement, settings.MaxRangeStatement);

                if (!possibleAnswers.Contains(wrongAnswer) && wrongAnswer != answer)
                {
                    possibleAnswers.Add(wrongAnswer);
                }
            }

            possibleAnswers.Insert(correctBox, answer);

            return possibleAnswers;
        }

        private void ConfigureBoxes(List<OptionBox> boxes, List<int> possibleAnswers, int correctBox)
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                boxes[i].Configure(i, possibleAnswers[i], i == correctBox);
            }
        }

        #endregion
    }
}