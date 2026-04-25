namespace Observatory.Photographer.UI
{
    public partial class TokenReferenceForm : Form
    {
        public TokenReferenceForm(TextBox relatedTextBox)
        {
            _relatedTextBox = relatedTextBox;
            InitializeComponent();
            BuildTokenLinks();
        }

        private TextBox _relatedTextBox;

        private void BuildTokenLinks()
        {
            var headerFont = new Font(Font.FontFamily, Font.Size + 2, FontStyle.Bold);
            LinkLayoutPanel.Controls.Add(
                new Label()
                {
                    Text = "Screenshot Tokens:",
                    AutoSize = true,
                    Font = headerFont,
                }
            );
            LinkLabel? finalLink = null;
            foreach (var token in ScreenshotTokens)
            {
                var linkLabel = new LinkLabel { Text = token, AutoSize = true };
                linkLabel.LinkClicked += LinkClicked;
                LinkLayoutPanel.Controls.Add(linkLabel);
                finalLink = linkLabel;
            }
            LinkLayoutPanel.SetFlowBreak(finalLink ?? new LinkLabel(), true);
            LinkLayoutPanel.Controls.Add(
                new Label()
                {
                    Text = "Status Tokens (Not Always Available):",
                    AutoSize = true,
                    Font = headerFont,
                }
            );
            foreach (var token in StatusTokens)
            {
                var linkLabel = new LinkLabel { Text = token, AutoSize = true };
                linkLabel.LinkClicked += LinkClicked;
                LinkLayoutPanel.Controls.Add(linkLabel);
            }
        }

        private void LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            if (sender is LinkLabel linkLabel)
            {
                var insertText = linkLabel.Text.Split(" - ")[0];
                var selectionStart = _relatedTextBox.SelectionStart;
                _relatedTextBox.Text = _relatedTextBox.Text.Insert(selectionStart, insertText);
                _relatedTextBox.SelectionStart = selectionStart + insertText.Length;
                _relatedTextBox.Focus();
            }
        }

        private static List<string> ScreenshotTokens =
        [
            "{cmdr} - Commander name",
            "{latitude} - Latitude coordinate",
            "{longitude} - Longitude coordinate",
            "{system} - System name",
            "{body} - Body name",
            "{altitude} - Altitude",
            "{heading} - Heading direction",
            "{timestamp} - ISO timestamp (YYYY-MM-DDTHH-MM-SS)",
            "{timestamp:FORMAT} - Custom formatted timestamp",
        ];

        private static List<string> StatusTokens =
        [
            "{guifocus} - Current GUI focus",
            "{balance} - Credit balance",
            "{cargo} - Cargo amount",
            "{mainfuel} - Main fuel level",
            "{reservoirfuel} - Reservoir fuel level",
            "{health} - Health percentage",
            "{oxygen} - Oxygen percentage",
            "{destination} - Destination name",
            "{gravity-g} - Gravity in G",
            "{gravity-mps2} - Gravity in m/s²",
            "{legalstate} - Legal state",
            "{radius} - Planet radius in km",
            "{temperature} - Temperature",
            "{hud} - HUD mode (Analysis/Combat)",
            "{docked} - Docked/Undocked",
            "{landed} - Landed/In Flight",
            "{landinggear} - Landing gear (Down/Raised)",
            "{shields} - Shields (Up/Down)",
            "{supercruise} - Supercruise/Normal Space",
            "{faoff} - Flight assist (On/Off)",
            "{hardpoints} - Hardpoints (Deployed/Retracted)",
            "{wing} - Wing/Solo",
            "{lights} - Lights (On/Off)",
            "{cargoscoop} - Cargo scoop (Deployed/Retracted)",
            "{silentrunning} - Silent running status",
            "{fuelscooping} - Fuel scooping status",
            "{srvbrake} - SRV brake (On/Off)",
            "{srvturret} - SRV turret (Active/Fixed)",
            "{srvproximity} - SRV proximity (Close/Clear)",
            "{srvdriveassist} - SRV drive assist (On/Off)",
            "{masslock} - Mass lock status",
            "{fsdcharging} - FSD charging/idle",
            "{fsdcooldown} - FSD cooldown/ready",
            "{lowfuel} - Low fuel warning",
            "{overheat} - Overheat warning",
            "{latlongvalid} - Lat/Long validity",
            "{indanger} - In danger status",
            "{interdiction} - Interdiction status",
            "{mainship} - In main ship",
            "{fighter} - In fighter",
            "{srv} - In SRV",
            "{nightvision} - Night vision (On/Off)",
            "{radialaltitude} - Altitude mode (Radial/Terrain)",
            "{fsdjump} - FSD jumping status",
            "{srvhighbeam} - SRV high beam (On/Off)",
            "{onfoot} - On foot status",
            "{intaxi} - In taxi status",
            "{inmulticrew} - In multicrew",
            "{onfootinstation} - On foot in station",
            "{onfootonplanet} - On foot on planet",
            "{aimdownsight} - Aiming down sight",
            "{lowoxygen} - Low oxygen warning",
            "{lowhealth} - Low health warning",
            "{cold} - Cold status",
            "{hot} - Hot status",
            "{verycold} - Very cold status",
            "{veryhot} - Very hot status",
            "{glidemode} - Gliding status",
            "{onfootinhangar} - On foot in hangar",
            "{onfootinsocialspace} - On foot in social space",
            "{onfootexterior} - On foot exterior",
            "{breathableatmosphere} - Breathable atmosphere",
            "{telepresencemulticrew} - Telepresence multicrew",
            "{physicalmulticrew} - Physical multicrew",
            "{fsdhyperdrivecharging} - FSD hyperdrive charging",
        ];
    }
}
