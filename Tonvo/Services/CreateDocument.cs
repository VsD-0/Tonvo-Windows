using IronPdf;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;

namespace Tonvo.Services
{
    internal static class CreateDocument
    {
        public static void Applicant(ApplicantModel applicantModel)
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            path = path.Replace("\\bin\\Debug\\net8.0-windows\\Tonvo.dll", "\\Resources\\Pictures\\FullLogo transparency2.png");

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Save Pdf File";
            saveFileDialog1.DefaultExt = "pdf";
            saveFileDialog1.Filter = "Pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            string PdfText = $"""
        <img align="center" src="{path}" height="150px" width="280px" alt="Не удалось вывести фото">
        <h1><b>{applicantModel.Surname} {applicantModel.Name}</b></h1>
        <p>
        Электронная почта: {applicantModel.Email}
        <br>
        Дата рождения: {applicantModel.BirthDate}
        </p>
        <hr>
        <p>
        {applicantModel.DesiredProfession}
        <br>
        Желаемая заработная плата: {applicantModel.DesiredSalary} ₽
        <br>
        Опыт работы: {applicantModel.Experience} л.
        </p>
        <hr>
        <p>
        Обо мне: {applicantModel.Information}
        </p> 
        """;

            if (saveFileDialog1.ShowDialog() == true)
            {
                string filename = saveFileDialog1.FileName;
                // actual code that will create Pdf files
                var HtmlLine = new HtmlToPdf();
                HtmlLine.RenderHtmlAsPdf(PdfText).SaveAs(filename);
                // MessageBox to display that file save
                MessageBox.Show($"Файл успешно сохранен!\n{saveFileDialog1.FileName}");
            }
        }
        public static void Vacancy(VacancyModel vacancyModel)
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            path = path.Replace("\\bin\\Debug\\net8.0-windows\\Tonvo.dll", "\\Resources\\Pictures\\FullLogo transparency2.png");
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Save Pdf File";
            saveFileDialog1.DefaultExt = "pdf";
            saveFileDialog1.Filter = "Pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            string PdfText = $"""
        <img align="center" src="{path}" height="150px" width="280px" alt="Не удалось вывести фото">
        <h1><b>{vacancyModel.Company}</b></h1>
        <p>
        Номер телефона: {vacancyModel.PhoneNumber}
        <br>
        Место работы: {vacancyModel.Address}
        </p>
        <hr>
        <p>
        {vacancyModel.Profession} 
        <br>
        Уровень дохода: {vacancyModel.Salary} ₽
        <br>
        Опыт работы от {vacancyModel.DesiredExperience}
        </p>
        <hr>
        <p>
        Подробная информация: {vacancyModel.Information}
        </p> 
        """;

            if (saveFileDialog1.ShowDialog() == true)
            {
                string filename = saveFileDialog1.FileName;
                // actual code that will create Pdf files
                var HtmlLine = new HtmlToPdf();
                HtmlLine.RenderHtmlAsPdf(PdfText).SaveAs(filename);
                // MessageBox to display that file save
                MessageBox.Show($"Файл успешно сохранен!\n{saveFileDialog1.FileName}");
            }
        }
    }
}
