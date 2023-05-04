using TMPro;
using Utils;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Controller
{
    public class UiController : MonoBehaviour
    {
        [SerializeField] private TMP_InputField InputField;
        [SerializeField] private Button GenerateButton;
        [SerializeField] private Button ClearButton;
        [SerializeField] private GameObject ErrorMessage;
        [SerializeField] private GameObject InfoMessage;

        public event Action<int> OnGenerateButtonClicked;
        public event Action OnClearButtonClicked;

        private int Value;

        private void Start()
        {
            GenerateButton.onClick.AddListener(Generate);
            ClearButton.onClick.AddListener(Clear);
            InputField.onValueChanged.AddListener(OnInputValidation);
        }

        private void Generate()
        {
            if (Value > 0 && Value < int.MaxValue)
            {
                ShowMessage(InfoMessage);
                OnGenerateButtonClicked?.Invoke(Value);
                return;
            }

            ShowMessage(ErrorMessage);
        }

        private void Clear()
        {
            OnClearButtonClicked?.Invoke();
        }

        private void OnInputValidation(string text)
        {
            Regex regex = new Regex("^\\d+$");

            if (regex.IsMatch(text) == false)
            {
                InputField.text = string.Empty;
                ShowMessage(ErrorMessage);
                return;
            }

            int.TryParse(text, out Value);
        }

        private async void ShowMessage(GameObject message)
        {
            message.SetActive(true);
            await Task.Delay(Configuration.UI_MESSAGE_TIMER);
            message.SetActive(false);
        }
    }
}