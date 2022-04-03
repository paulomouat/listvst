using System;
using Xunit;
using Xunit.Abstractions;

namespace ListVst.Tests
{
    public class UnitTest1
    {
	    private readonly ITestOutputHelper _testOutputHelper;

	    public UnitTest1(ITestOutputHelper testOutputHelper)
	    {
		    _testOutputHelper = testOutputHelper;
	    }

	    [Fact]
        public void Test1()
        {
			_testOutputHelper.WriteLine("1");
        }
        
        [Fact]
        public void Test2()
        {
	        _testOutputHelper.WriteLine("2");
        }

        private const string TextXml = @"
<ChannelGroup name=""AudioOutput"" flags=""1"">
			<AudioOutputChannel gain=""1"" pan=""0.5"" name=""Channel01"" label=""Main"" mute=""0"" solo=""0"" soloSafe=""0""
			                    color=""￹00000000"">
				<SpeakerSetup x:id=""speakerType"" type=""Stereo""/>
				<UID x:id=""uniqueID"" uid=""{0847B9D3-9358-D144-A4C7-630DB50F0762}""/>
				<Attributes x:id=""Automation"" mode=""0""/>
				<Attributes x:id=""Inserts"">
					<Attributes name=""FX02"">
						<UID x:id=""uniqueID"" uid=""{20BE7409-7D6A-004E-B048-03B5678FF0F3}""/>
						<UID x:id=""deviceClassID"" uid=""{61756678-4952-5350-314C-505342414C4B}""/>
						<Attributes x:id=""deviceData"" name=""SPL IRON"">
							<UID x:id=""uniqueID"" uid=""{E0728D83-687E-0544-AF41-7A785AD26DA3}""/>
						</Attributes>
						<Attributes x:id=""ghostData"" presetType=""aupreset"">
							<Attributes x:id=""classInfo"" classID=""{61756678-4952-5350-314C-505342414C4B}"" name=""SPL IRON""
							            category=""AudioEffect"" subCategory=""AudioUnit""/>
							<Attributes x:id=""IO"">
								<List x:id=""audioInputs"">
									<PortDescription flags=""3"" subType=""3"" label=""In 1""/>
									<PortDescription type=""1"" subType=""3"" label=""Sidechain""/>
								</List>
								<List x:id=""audioOutputs"">
									<PortDescription flags=""3"" subType=""3"" label=""Out 1""/>
								</List>
							</Attributes>
						</Attributes>
						<Attributes x:id=""Automation"" mode=""0""/>
						<Attributes x:id=""Presets"" pname=""default"" dirty=""1""/>
						<String x:id=""presetPath"" text=""Presets/Channels/Main/1 - SPL IRON.aupreset""/>
					</Attributes>
					<Attributes name=""FX03"">
						<UID x:id=""uniqueID"" uid=""{16C88B3C-3137-7C4C-B5F7-1BA3AA7A2CDA}""/>
						<UID x:id=""deviceClassID"" uid=""{56535442-4148-3262-6C61-636B20626F78}""/>
						<Attributes x:id=""deviceData"" name=""Black Box Analog Design HG-2"">
							<UID x:id=""uniqueID"" uid=""{37643A27-13DE-5447-AB08-0761FF06558B}""/>
						</Attributes>
						<Attributes x:id=""ghostData"" presetType=""vstpreset"">
							<Attributes x:id=""classInfo"" classID=""{56535442-4148-3262-6C61-636B20626F78}"" name=""Black Box Analog Design HG-2""
							            category=""AudioEffect"" subCategory=""VST3/Distortion""/>
							<Attributes x:id=""IO"">
								<List x:id=""audioInputs"">
									<PortDescription flags=""3"" subType=""3"" label=""Stereo In""/>
								</List>
								<List x:id=""audioOutputs"">
									<PortDescription flags=""3"" subType=""3"" label=""Stereo Out""/>
								</List>
							</Attributes>
						</Attributes>
						<Attributes x:id=""Automation"" mode=""0""/>
						<Attributes x:id=""Presets"" pname=""default"" dirty=""1""/>
						<String x:id=""presetPath"" text=""Presets/Channels/Main/2 - Black Box Analog Design HG-2.vstpreset""/>
					</Attributes>
					<Attributes name=""FX01"">
						<UID x:id=""uniqueID"" uid=""{45979D61-104D-7147-BA1D-857009F13DD2}""/>
						<UID x:id=""deviceClassID"" uid=""{61756678-6571-6479-5464-6E5342414C4B}""/>
						<Attributes x:id=""deviceData"" name=""Gullfoss"">
							<UID x:id=""uniqueID"" uid=""{BDB36919-1DC5-814C-B73A-0E2436B16987}""/>
						</Attributes>
						<Attributes x:id=""ghostData"" presetType=""aupreset"">
							<Attributes x:id=""classInfo"" classID=""{61756678-6571-6479-5464-6E5342414C4B}"" name=""Gullfoss""
							            category=""AudioEffect"" subCategory=""AudioUnit""/>
							<Attributes x:id=""IO"">
								<List x:id=""audioInputs"">
									<PortDescription flags=""3"" subType=""3"" label=""In 1""/>
								</List>
								<List x:id=""audioOutputs"">
									<PortDescription flags=""3"" subType=""3"" label=""Out 1""/>
								</List>
							</Attributes>
						</Attributes>
						<Attributes x:id=""Automation"" mode=""0""/>
						<Attributes x:id=""Presets"" pname=""default"" dirty=""1""/>
						<String x:id=""presetPath"" text=""Presets/Channels/Main/3 - Gullfoss.aupreset""/>
					</Attributes>
					<Attributes x:id=""Presets"" pname=""default"" dirty=""0""/>
					<Attributes x:id=""Combinator"" name=""Combinator"">
						<UID x:id=""uniqueID"" uid=""{E1AA3B01-26E5-184D-A2C3-0FBB50D5B2F6}""/>
					</Attributes>
				</Attributes>
				<Attributes x:id=""PostFaderInserts"">
					<Attributes name=""FX02"">
						<UID x:id=""uniqueID"" uid=""{FB814DF2-239A-EF49-8D48-C2977C6EFA0C}""/>
						<UID x:id=""deviceClassID"" uid=""{AFD92F72-9A04-47B7-B5E8-D1D568DEA985}""/>
						<Attributes x:id=""deviceData"" name=""FabFilter Pro-L 2"">
							<UID x:id=""uniqueID"" uid=""{6BA0EC56-1A10-6342-97A5-CEFE87E0DFE4}""/>
						</Attributes>
						<Attributes x:id=""ghostData"" presetType=""vstpreset"">
							<Attributes x:id=""classInfo"" classID=""{AFD92F72-9A04-47B7-B5E8-D1D568DEA985}"" name=""FabFilter Pro-L 2""
							            category=""AudioEffect"" subCategory=""VST3/Dynamics""/>
							<Attributes x:id=""IO"">
								<List x:id=""audioInputs"">
									<PortDescription flags=""3"" subType=""3"" label=""Stereo In""/>
									<PortDescription type=""1"" subType=""3"" label=""Stereo Side Chain""/>
								</List>
								<List x:id=""audioOutputs"">
									<PortDescription flags=""3"" subType=""3"" label=""Stereo Out""/>
								</List>
								<List x:id=""musicInputs"">
									<PortDescription flags=""3"" label=""Midi In""/>
								</List>
							</Attributes>
						</Attributes>
						<Attributes x:id=""Automation"" mode=""0""/>
						<Attributes x:id=""Presets"" pname=""default"" dirty=""1""/>
						<String x:id=""presetPath"" text=""Presets/Channels/Main/1 - FabFilter Pro-L 2.vstpreset""/>
					</Attributes>
					<Attributes name=""FX01"" expanded=""1"">
						<UID x:id=""uniqueID"" uid=""{2DFF15F7-D0D6-F54C-B228-DD20916CD611}""/>
						<UID x:id=""deviceClassID"" uid=""{B05B181C-6417-4441-A6F4-CC3E780548AD}""/>
						<Attributes x:id=""deviceData"" name=""VU Meter"">
							<UID x:id=""uniqueID"" uid=""{A601C97D-1081-474A-BBD7-72E8AC542453}""/>
						</Attributes>
						<Attributes x:id=""ghostData"" presetType=""dsppreset"">
							<Attributes x:id=""classInfo"" classID=""{B05B181C-6417-4441-A6F4-CC3E780548AD}"" name=""VU Meter""
							            category=""AudioEffect"" subCategory=""(Native)/Analysis""/>
							<Attributes x:id=""IO"">
								<List x:id=""audioInputs"">
									<PortDescription subType=""3"" label=""Input""/>
								</List>
								<List x:id=""audioOutputs"">
									<PortDescription subType=""3"" label=""Output""/>
								</List>
							</Attributes>
						</Attributes>
						<Attributes x:id=""Automation"" mode=""0""/>
						<Attributes x:id=""Presets"" pname=""default"" dirty=""1""/>
						<String x:id=""presetPath"" text=""Presets/Channels/Main/2 - VU Meter.dsppreset""/>
					</Attributes>
					<Attributes name=""FX03"" expanded=""1"">
						<UID x:id=""uniqueID"" uid=""{F344CA25-222F-7443-8250-6268C56846E9}""/>
						<UID x:id=""deviceClassID"" uid=""{0C6C94DF-CCE0-465C-886A-F1F01A142104}""/>
						<Attributes x:id=""deviceData"" name=""Spectrum Meter"">
							<UID x:id=""uniqueID"" uid=""{CB18A933-ABF4-F74D-859B-78FE0DCE8206}""/>
						</Attributes>
						<Attributes x:id=""ghostData"" presetType=""fxpreset"">
							<Attributes x:id=""classInfo"" classID=""{0C6C94DF-CCE0-465C-886A-F1F01A142104}"" name=""Spectrum Meter""
							            category=""AudioEffect"" subCategory=""(Native)/Analysis""/>
							<Attributes x:id=""IO"">
								<List x:id=""audioInputs"">
									<PortDescription flags=""3"" subType=""3"" label=""Input""/>
									<PortDescription type=""1"" subType=""3"" label=""SC""/>
								</List>
								<List x:id=""audioOutputs"">
									<PortDescription flags=""3"" subType=""3"" label=""Output""/>
								</List>
							</Attributes>
						</Attributes>
						<Attributes x:id=""Automation"" mode=""0""/>
						<Attributes x:id=""Presets"" pname=""default"" dirty=""1"">
							<Attributes x:id=""url"" type=""1"" url=""file:///Applications/Studio One 4.app/Contents/Presets/PreSonus/Spectrum Meter/default.preset""/>
						</Attributes>
						<String x:id=""presetPath"" text=""Presets/Channels/Main/3 - Spectrum Meter.fxpreset""/>
					</Attributes>
					<Attributes name=""FX04"">
						<UID x:id=""uniqueID"" uid=""{FA42CFDF-9981-5648-AF6D-A9B14B02E505}""/>
						<UID x:id=""deviceClassID"" uid=""{61756678-4241-5A4E-7074-5A6942414C4B}""/>
						<Attributes x:id=""deviceData"" name=""Tonal Balance Control"">
							<UID x:id=""uniqueID"" uid=""{7B456F04-3EC9-3245-81AA-6BEFB376BA73}""/>
						</Attributes>
						<Attributes x:id=""ghostData"" presetType=""aupreset"">
							<Attributes x:id=""classInfo"" classID=""{61756678-4241-5A4E-7074-5A6942414C4B}"" name=""Tonal Balance Control""
							            category=""AudioEffect"" subCategory=""AudioUnit""/>
							<Attributes x:id=""IO"">
								<List x:id=""audioInputs"">
									<PortDescription flags=""3"" subType=""3"" label=""In 1""/>
								</List>
								<List x:id=""audioOutputs"">
									<PortDescription flags=""3"" subType=""3"" label=""Out 1""/>
								</List>
							</Attributes>
						</Attributes>
						<Attributes x:id=""Automation"" mode=""0""/>
						<Attributes x:id=""Presets"" pname=""default"" dirty=""0""/>
						<String x:id=""presetPath"" text=""Presets/Channels/Main/4 - Tonal Balance Control.aupreset""/>
					</Attributes>
					<Attributes x:id=""Presets"" pname=""default"" dirty=""0""/>
					<Attributes x:id=""Combinator"" name=""Combinator"">
						<UID x:id=""uniqueID"" uid=""{0749A4E0-02C6-9649-80D9-8DDC1A7853D9}""/>
					</Attributes>
				</Attributes>
				<Attributes x:id=""AudioClick"" on=""1"" gain=""1""/>
				<Attributes x:id=""MacroControls"" showMacroPads=""0"" presentation=""0""/>
				<Attributes x:id=""AudioBusMixFX"" passThrough=""1"">
					<Attributes name=""FX01"">
						<UID x:id=""uniqueID"" uid=""{B5F2EA8B-5FF4-6346-8B43-48BF713381A6}""/>
						<UID x:id=""deviceClassID"" uid=""{160C9CD8-E378-4014-96AF-6F6E34F174A1}""/>
						<Attributes x:id=""deviceData"" name=""Tape Multi Track"">
							<UID x:id=""uniqueID"" uid=""{B57B41C4-3A6B-C44E-9ED8-2367BD795506}""/>
						</Attributes>
						<Attributes x:id=""ghostData"" presetType=""vstpreset"">
							<Attributes x:id=""classInfo"" classID=""{160C9CD8-E378-4014-96AF-6F6E34F174A1}"" name=""Tape Multi Track""
							            category=""AudioMixEffect"" subCategory=""VST3/Distortion""/>
							<Attributes x:id=""IO"">
								<List x:id=""audioInputs"">
									<PortDescription flags=""3"" subType=""3"" label=""Stereo In""/>
								</List>
								<List x:id=""audioOutputs"">
									<PortDescription flags=""3"" subType=""3"" label=""Stereo Out""/>
								</List>
							</Attributes>
						</Attributes>
						<Attributes x:id=""Automation"" mode=""0""/>
						<Attributes x:id=""Presets"" pname=""default"" dirty=""1""/>
						<String x:id=""presetPath"" text=""Presets/Channels/Main/1 - Tape Multi Track.vstpreset""/>
					</Attributes>
					<Attributes x:id=""Presets"" pname=""default"" dirty=""0""/>
				</Attributes>
			</AudioOutputChannel>
		</ChannelGroup>";
    }
}
