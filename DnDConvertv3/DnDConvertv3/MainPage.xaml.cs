using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DnDConvertv3
{
    public partial class MainPage : ContentPage
    {
        decimal remainderCopper = 0;
        decimal copper;
        decimal silver;
        decimal electrum;
        decimal gold;
        decimal platinum;
        int teamSize;
        decimal[] objectCurrencyValues = new decimal[7];

        //calculate button clicked event
        public void RecalculateResult(object sender, EventArgs e)
        {

            //if user backspaces, copper default is 0
            if (String.IsNullOrEmpty(copperEntry.Text) == true)
            {
                copper = 0;
            }
            else
            {
                copper = Convert.ToDecimal(copperEntry.Text);
            }

            //if user backspaces, silver default is 0
            if (String.IsNullOrEmpty(silverEntry.Text) == true)
            {
                silver = 0;
            }
            else
            {
                silver = Convert.ToDecimal(silverEntry.Text);
            }

            //if user backspaces, electrum default is 0
            if (String.IsNullOrEmpty(electrumEntry.Text) == true)
            {
                electrum = 0;
            }
            else
            {
                electrum = Convert.ToDecimal(electrumEntry.Text);
            }

            //if user backspaces, gold default is 0
            if (String.IsNullOrEmpty(goldEntry.Text) == true)
            {
                gold = 0;
            }
            else
            {
                gold = Convert.ToDecimal(goldEntry.Text);
            }

            //if user backspaces, platinum default is 0
            if (String.IsNullOrEmpty(platinumEntry.Text) == true)
            {
                platinum = 0;
            }
            else
            {
                platinum = Convert.ToDecimal(platinumEntry.Text);
            }

            //if user backspaces, team size default is 1
            if (String.IsNullOrEmpty(teamSizeEntry.Text) == true || teamSizeEntry.Text == "0")
            {
                teamSize = 1;
            }
            else
            {
                teamSize = Convert.ToInt32(teamSizeEntry.Text);
            }

            CalculateResultModified(copper, silver, electrum, gold, platinum, teamSize, objectCurrencyValues);

            //start making objects and fill list
            decimal numberOfObjectsWithoutExtraCopper = objectCurrencyValues[5];
            string numberOfObjectsWithoutExtraCopperString = numberOfObjectsWithoutExtraCopper.ToString();
            decimal numberOfObjectsWithExtraCopper = objectCurrencyValues[3];
            string numberOfObjectsWithExtraCopperString = numberOfObjectsWithExtraCopper.ToString();

            decimal objectCopper = objectCurrencyValues[0];
            string objectCopperString = objectCopper.ToString();
            decimal objectCopperForThoseWithoutExtra = objectCurrencyValues[4];
            string objectCopperForThoseWithoutExtraString = objectCopperForThoseWithoutExtra.ToString();

            decimal objectSilver = objectCurrencyValues[1];
            string objectSilverString = objectSilver.ToString();

            decimal objectGold = objectCurrencyValues[2];
            string objectGoldString = objectGold.ToString();

            List<CurrencyResult> resultListSource = new List<CurrencyResult>();    //list that stores currency result objects, and is used as listview itemsSource in "resultsPage"


            for (int i = 0; i < 1; i++)
            {

                resultListSource.Add(new CurrencyResult()
                {
                    copper = objectCopperString,
                    silver = objectSilverString,
                    electrum = "0",
                    gold = objectGoldString,
                    platinum = "0",
                    totalCurrencyString = "(" + numberOfObjectsWithExtraCopperString + ") " + objectGoldString + "gp " + objectSilverString + "sp " + objectCopperString + "cp"
                });
            }


            if (remainderCopper > 0)
            {
                for (int i = 0; i < 1; i++)
                {
                    resultListSource.Add(new CurrencyResult()
                    {
                        copper = objectCopperForThoseWithoutExtraString,
                        silver = objectSilverString,
                        electrum = "0",
                        gold = objectGoldString,
                        platinum = "0",
                        totalCurrencyString = "(" + numberOfObjectsWithoutExtraCopperString + ") " + objectGoldString + "gp " + objectSilverString + "sp " + objectCopperForThoseWithoutExtraString + "cp"
                    });
                }
            }


            Navigation.PushAsync(new ResultsPage(resultListSource));


        }


        //Calculates final amount of each currency divided by team members. The output be in only copper, silver, and gold
        public decimal[] CalculateResultModified(decimal copper, decimal silver, decimal electrum, decimal gold, decimal platinum, int teamSize, decimal[] array)
        {
            decimal amountForMembersWithExtraCopper = 0;
            int numberOfMembersWithExtraCopper = 0;
            int numberOfMembersWithoutExtraCopper = 0;
            decimal amountForMembersWithoutExtraCopper = 0;


            decimal totalSilver = 0;
            decimal eachSilver = 0;
            decimal remainderSilver = 0;

            decimal totalCopper = 0;
            decimal eachCopper = 0;

            remainderCopper = 0;

            //ex teamSize = 3
            decimal platConvertedToGold = platinum * 10;                                             //ex. 1 pp = 10gp
            decimal electrumConvertedToGold = electrum * Convert.ToDecimal(0.5);                                        //ex. 2 ep = 1gp
            decimal silverConvertedToGold = silver * Convert.ToDecimal(0.1);                                            //ex. 35 sp = 3.5gp
            decimal copperConvertedToGold = copper * Convert.ToDecimal(0.01);                                           //ex. 60 cp = .6gp

            decimal totalGold = (gold + platConvertedToGold + electrumConvertedToGold + silverConvertedToGold + copperConvertedToGold) / teamSize;                  //ex. 10.7gp

            decimal eachGold = Math.Floor(totalGold);                    //how much gold each person gets. The whole number portion of totalGold.            //ex. 10gp
            decimal remainderGold = totalGold - eachGold;                //how much gold is left. Decimal portion of totalGold.                              //ex. .70gp

            if (remainderGold > 0)                                       //if we still have money to distribute
            {
                totalSilver = remainderGold * 10;                                                                                                           //ex. 7.00sp
                eachSilver = Math.Floor(totalSilver);                  //how much silver each person gets. The whole number portion of totalSilver.         //ex. 7sp
                remainderSilver = totalSilver - eachSilver;            //how much silver is left. Decimal portion of totalSilver.                           //ex. 0sp
            }

            if (remainderSilver > 0)
            {
                totalCopper = remainderSilver * 10;
                eachCopper = Math.Floor(totalCopper);
                remainderCopper = totalCopper - eachCopper;
            }

            if (remainderCopper > 0)
            {

                decimal copperLeftOver = remainderCopper * teamSize;
                copperLeftOver += (eachCopper * teamSize);
                copperLeftOver = Math.Round(copperLeftOver);


                int[] teamMembers = new int[teamSize];
                for (int i = 0; i < teamSize; i++)                   //fill array with 0s
                {
                    teamMembers[i] = 0;
                }

                int count = 0;
                while (copperLeftOver != 0)
                {
                    teamMembers[count] += 1;
                    copperLeftOver -= 1;
                    count += 1;
                    if (count == teamSize)
                        count = 0;
                }

                for (int i = 0; i < teamSize - 1; i++)
                {
                    if (teamMembers[i] != teamMembers[i + 1])
                    {
                        numberOfMembersWithExtraCopper = i + 1;
                        amountForMembersWithExtraCopper = teamMembers[i];
                        numberOfMembersWithoutExtraCopper = teamSize - numberOfMembersWithExtraCopper;
                        amountForMembersWithoutExtraCopper = teamMembers[i + 1];
                    }

                }


                eachCopper = amountForMembersWithExtraCopper;//


                array[4] = amountForMembersWithoutExtraCopper;

            }




            if (numberOfMembersWithExtraCopper == 0)
            {
                numberOfMembersWithExtraCopper = teamSize;
            }


            array[0] = eachCopper;//
            //array[0] = amountForMembersWithExtraCopper;
            array[1] = eachSilver;
            array[2] = eachGold;

            array[3] = numberOfMembersWithExtraCopper;
            array[5] = numberOfMembersWithoutExtraCopper;

            return array;

        }


        public MainPage()
        {
            InitializeComponent();

            // get strings from currency entries and convert to int for calculations
            copper = Convert.ToInt32(copperEntry.Text);
            silver = Convert.ToInt32(silverEntry.Text);
            electrum = Convert.ToInt32(electrumEntry.Text);
            gold = Convert.ToInt32(goldEntry.Text);
            platinum = Convert.ToInt32(platinumEntry.Text);
            teamSize = Convert.ToInt32(teamSizeEntry.Text);
            if (teamSize == 0)
            {
                teamSize = 1;
            }

            //fill array with some currency values. it is updated everytime the entries change.
            CalculateResultModified(copper, silver, electrum, gold, platinum, teamSize, objectCurrencyValues);

        }

        private void Clear_Activated(object sender, EventArgs e)
        {
            copperEntry.Text = "";
            silverEntry.Text = "";
            electrumEntry.Text = "";
            goldEntry.Text = "";
            platinumEntry.Text = "";
            teamSizeEntry.Text = "";

        }
    }
}
