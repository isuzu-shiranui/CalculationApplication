using Prism.Commands;
using Plugin;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;

namespace CalculationApplication.ViewModels
{
    public class MainWindowViewModel : Commons.ViewModelBase
    {
        private double leftValue;
        [Required]
        [Range(0, double.MaxValue)]
        public double LeftValue
        {
            get { return leftValue; }
            set { SetProperty(ref leftValue, value); CalculationCommand.RaiseCanExecuteChanged(); }
        }

        private double rightValue;
        [Required]
        [Range(0, double.MaxValue)]
        public double RightValue
        {
            get { return rightValue; }
            set { SetProperty(ref rightValue, value); CalculationCommand.RaiseCanExecuteChanged(); }
        }

        private double resultValue;
        public double ResultValue
        {
            get { return resultValue; }
            set { SetProperty(ref resultValue, value); }
        }

        [Import(typeof(IPlugin))]//プラグインを"1つだけ"読み込む
        private IPlugin Plugin { get; set; }


        private DelegateCommand calculationCommand;
        public DelegateCommand CalculationCommand => calculationCommand ?? (new DelegateCommand(CalculationExecute, () => !HasErrors));

        private void CalculationExecute()
        {
            ResultValue = Plugin.Calculation(LeftValue, RightValue);
        }

        public MainWindowViewModel()
        {
            LoadPlugins();
        }

        /// <summary>
        /// プラグインを全て読みこんで、[Import]がついているプロパティに格納する。
        /// </summary>
        private void LoadPlugins()
        {
            //フォルダがなければ作る。
            string pluginsPath = Directory.GetCurrentDirectory() + @"\plugins";
            if (!Directory.Exists(pluginsPath)) Directory.CreateDirectory(pluginsPath);

            //プラグイン読み込み
            using (DirectoryCatalog catalog = new DirectoryCatalog(pluginsPath))
            using (CompositionContainer container = new CompositionContainer(catalog))
            {
                if (catalog.LoadedFiles.Count > 0) container.SatisfyImportsOnce(this);
            }
        }
    }
}
