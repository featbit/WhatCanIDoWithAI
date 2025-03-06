using KnolwedgeBase.DesktopApp.Components.Components.CodeGen;
using KnowledgeBase.Models;
using KnowledgeBase.Models.ReportGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Specification = KnowledgeBase.Models.ReportGenerator.Specification;

namespace KnolwedgeBase.DesktopApp.StateContainers
{
    public class CodeGenStateContainer
    {
        private string? _reportId;
        public string ReportId
        {
            get => _reportId ?? string.Empty;
            set
            {
                _reportId = value;
                NotifyStateChanged();
            }
        }

        private string? _codeRootPath;
        public string CodeRootPath
        {
            get => _codeRootPath ?? @"C:/Code/aicoding/softwarepatent";
            set
            {
                _codeRootPath = value;
                NotifyStateChanged();
            }
        }

        private int? _activeComponent;
        public int ActiveComponent
        {
            get => _activeComponent ?? 0;
            set
            {
                _activeComponent = value;
                NotifyStateChanged();
            }
        }

        private Specification? _specification;
        public Specification Specification
        {
            get => _specification ?? new Specification() {
                Features = new List<Feature>()
            };
            set
            {
                _specification = value;
                NotifyStateChanged();
            }
        }

        private CodeForReport? _codeForReport;
        public CodeForReport CodeForReport
        {
            get => _codeForReport ?? new CodeForReport() {
                CodeFeatures = new List<ReportCodeFeature>()
            };
            set
            {
                _codeForReport = value;
                NotifyStateChanged();
            }
        }


        private string? savedString;

        public string Property
        {
            get => savedString ?? string.Empty;
            set
            {
                savedString = value;
                NotifyStateChanged();
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
