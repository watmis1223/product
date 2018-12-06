using ProductCalculation.Library.Entity.PriceCalculation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductCalculation.Library.Business.PriceCalculation
{
    public class MarginCalculation
    {
        public void UpdateBaseAmountAll(CalculationModel model)
        {
            if (model.CalculationMarginViewItems == null)
            {
                return;
            }

            //if margin not enabled
            if (!model.GeneralSetting.Options.Contains("M"))
            {
                return;
            }

            //get margin item by tag
            var oModels = model.CalculationMarginViewItems.FindAll(item => !item.IsSummary);

            foreach (CalculationItemModel oMargin in oModels)
            {
                CalculationItemModel oBasic = model.CalculationViewItems.Where(o => o.Tag == oMargin.Tag).FirstOrDefault();

                if (oBasic != null)
                {
                    oMargin.Total = oBasic.Total;
                }
                else
                {
                    //if BEN tag
                    oMargin.Total = model.CalculationViewItems.Where(o => o.Tag.StartsWith("BEN")).Sum(o => o.Total);
                }

                //set variable total
                if (oMargin.Tag == "BEK" || oMargin.Tag == "VK(bar)" || oMargin.Tag == "VK(brutto)")
                {
                    //if base amount row
                    oMargin.VariableTotal = oMargin.Total;
                }
                else
                {
                    if (oMargin.EditedField == "P")
                    {
                        //calculate amount fix
                        oMargin.AmountFix = oMargin.Total * (oMargin.AmountPercent / 100);

                        //calculate variable total
                        oMargin.VariableTotal = oMargin.Total - oMargin.AmountFix;
                    }
                    else if (oMargin.EditedField == "F")
                    {
                        //calculate amount fix
                        oMargin.AmountPercent = (oMargin.AmountFix / oMargin.Total) * 100;

                        //calculate variable total
                        oMargin.VariableTotal = oMargin.Total - oMargin.AmountFix;
                    }
                    else if (oMargin.EditedField == "V")
                    {
                        //calculate amount fix
                        oMargin.AmountFix = oMargin.Total - oMargin.VariableTotal;

                        //calculate amount percent
                        oMargin.AmountPercent = (oMargin.AmountFix / oMargin.Total) * 100;
                    }
                    else
                    {
                        //calculate amount fix
                        oMargin.AmountFix = oMargin.Total * (oMargin.AmountPercent / 100);

                        //calculate variable total
                        oMargin.VariableTotal = oMargin.Total - oMargin.AmountFix;
                    }
                }
            }

            //update group amount
            var oGroups = model.CalculationMarginViewItems.FindAll(item => item.IsSummary);
            foreach (CalculationItemModel item in oGroups)
            {
                UpdateGroupAmount(model, item.Group, item.ItemOrder);
            }
        }

        void UpdateGroupAmount(CalculationModel model, int group, int groupID)
        {
            CalculationItemModel oGroup = model.CalculationMarginViewItems.Find(item => item.Group == group && item.ItemOrder == groupID && item.IsSummary);

            if (oGroup != null)
            {
                oGroup.VariableTotal = 0;
                oGroup.AmountFix = 0;

                //set Verwaltungs- und Vertriebskosten
                if (oGroup.Tag == "VVK")
                {
                    oGroup.Total = 0;
                }

                //update items in group list first
                foreach (int i in oGroup.SummaryGroups)
                {
                    //get items per group
                    var oModels = model.CalculationMarginViewItems.FindAll(item => item.Group == i && !item.IsSummary);

                    //update group's amout fix
                    oGroup.AmountFix += (from item in oModels select item.AmountFix).Sum();

                    //update group's variable total
                    oGroup.VariableTotal += (from item in oModels select item.VariableTotal).Sum();

                    if (oGroup.Tag == "VVK")
                    {
                        oGroup.Total += (from item in oModels select item.Total).Sum();
                    }
                }

                //set Deckungsbeitrag
                if (oGroup.Tag == "VK")
                {
                    oGroup.VariableTotal = oGroup.AmountFix;
                }
            }
        }

        public decimal GetBaseTotal(CalculationModel model, int rowID)
        {
            return model.CalculationMarginViewItems[rowID].Total;
        }

        public void UpdateMarginRowAmountFix(CalculationModel model, int rowID, decimal value)
        {
            //get margin item by tag
            CalculationItemModel oModel = model.CalculationMarginViewItems[rowID];

            if (oModel != null)
            {
                oModel.AmountFix = value;
            }
        }

        public void UpdateMarginRowAmountPercent(CalculationModel model, int rowID, decimal value)
        {
            //get margin item by tag
            CalculationItemModel oModel = model.CalculationMarginViewItems[rowID];

            if (oModel != null)
            {
                oModel.AmountPercent = value;
            }
        }

        public void UpdateMarginRowAmountVariable(CalculationModel model, int rowID, decimal value)
        {
            //get margin item by tag
            CalculationItemModel oModel = model.CalculationMarginViewItems[rowID];

            if (oModel != null)
            {
                oModel.VariableTotal = value;
            }
        }

        public void UpdateMarginRowEditedField(CalculationModel model, int rowID, string field)
        {
            //P percent, F fix, V variable total, T total

            //get margin item by tag
            CalculationItemModel oModel = model.CalculationMarginViewItems[rowID];

            if (oModel != null)
            {
                oModel.EditedField = field;
            }
        }

        public decimal GetMarginSummarize(CalculationModel model)
        {
            decimal iSummary = 0;

            CalculationItemModel oVK = model.CalculationMarginViewItems.Find(item => item.Tag == "VK");
            CalculationItemModel oVKbrutto = model.CalculationMarginViewItems.Find(item => item.Tag == "VK(brutto)");

            if (oVK != null && (oVKbrutto != null && oVKbrutto.VariableTotal > 0))
            {
                iSummary = (oVK.VariableTotal / oVKbrutto.VariableTotal) * 100;
            }

            return iSummary;
        }
    }
}
