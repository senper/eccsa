import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Grid, Paper } from "@material-ui/core";
import { Link } from "react-router-dom";
import Breadcrumbs from "@material-ui/core/Breadcrumbs";
import "bootstrap/dist/css/bootstrap.min.css";
import "../App.css";
import "../component/slider/HomeSlider";
import QuickLinks from "../component/NavMenus/QuickLinks";
import "bootstrap/dist/css/bootstrap.css";
import Footer from "../component/Footer/Footer";

const useStyle = makeStyles((theme) => ({
  grid: {
    width: "100%",
    margin: "0px",
    padding: "3px",
  },
  paper: {
    padding: theme.spacing(2),
    textAlign: "center",
    color: theme.palette.text.secondary,
    background: theme.palette.success.light,
  },
  breadcrum: {
    backgroundColor: "#fff",
    padding: "10px",
    marginLeft: "100px",
  },
}));
function handleClick(event) {
  event.preventDefault();
}
function App() {
  const classes = useStyle();
  return (
    <div class="mmenu">
      {/* <img

        className="d-block w-70"
        class="img-fluid"
        src="../images/header.jpg"
        href="/"></img>  <a href="/login" class="btn btn-outline-success">Login/Register</a>
      <MainMenu/> */}

      <Grid container spacing={2} className="grid">
        <Grid item xs={12} md={12}>
          {/* <MainMenu/> */}

          <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrum}>
            <Link to="/"> Home</Link>
            <Link eventKey="about" aria-current="page" to="#">
              {" "}
              About ECCSA
            </Link>
          </Breadcrumbs>
        </Grid>
        <Grid item xs={1} md={1}>
          {/* <Welcome /> */}
        </Grid>
        <Grid item xs={11} md={8}>
          <Paper>
            <div class="row">
              <section id="team" class="pb-5">
                <div class="container">
                  <h5 class="section-title h3">About The Chamber </h5>
                  <div class="row">
                  <h3>History of the Ethiopian Chamber of Commerce</h3>

<p>The first chamber of commerce in the history of Ethiopia was established pursuant to General Notice No. 90/1947 which clearly defined the functions and duties of chamber and membership was voluntary. In 1971 the Ethiopian Chamber of Commerce (ECC) has been reorganized with branches in Asmara, Dire Dawa, Jimma, Gonder &amp; Nazareth. Following the change of government in 1974, Proclamation No. 148/1974, which was basically a public law, was promulgated and the Ethiopian chamber of commerce was restructured in line with centrally planned economic policy of the then government. Membership had become mandatory. In 2003, Proclamation No. 341/2003 the re-constitute of chambers of commerce was based on Developmental Democratic System. Accordingly, in line with this the Ethiopian Chamber of Commerce has been restructured and renamed as the Ethiopian Chamber of Commerce &amp; Sectoral Associations (ECCSA).&nbsp;</p>

<h3>Establishment of ECCSA</h3>

<p>ECCSA is an Autonomous, Non-for-Profit, Non-Partisan, Private Sector institution and membership based organization. ECCSA was established with the aim of projecting trade and investment, inter alia, in a bid to creating a vibrant private sector and business friendly environment, working in cooperation with the government, business community, development partners and stakeholders.&nbsp;<br />
ECCSA has eighteen members including nine Regional Chambers of Commerce and Sectoral Associations, two City Chambers of Commerce and Sectoral Associations, one National Chamber of Sectoral Association and six Sectoral Associations organized at national level.&nbsp;<br />
The higher decision making body at the Chamber system is the General Assembly (GA) which is composed of the representatives of member chambers of commerce and sectoral associations of ECCSA. The GA elects the president, the vice president and nine Board of Directors every two years. A management team led by the secretariat manages the day to day operation of the chamber.<br />
ECCSA Objectives</p>

<p>According to the proclamation 341/2003 article 3, Ethiopian Chamber of Commerce and Sectoral Associations have the following objectives:-</p>

<ul>
	<li>To provide different services to the business community;</li>
	<li>To safeguard the overall rights and benefits of its members;</li>
	<li>To promote and publicize products and services of the country;</li>
	<li>To enhance trade and investment of the country;</li>
	<li>To serve as a bridge between the business community and the government.</li>
</ul>

<h3> Mission </h3>

<p>To head the creation of enabling business environment and capacity in which the business community plays a leading role in the economy through evidence based dialogue and partnership.&nbsp;</p>

<h3> Vision </h3>

<p>To see a dynamic Chamber system proactively responsive towards sustainable private sector growth&nbsp;<br />
</p> 
<h3>Core Values</h3>
<ul>
	<li>o&nbsp;&nbsp; &nbsp;Transparency</li>
	<li>o&nbsp;&nbsp; &nbsp;Accountability</li>
	<li>o&nbsp;&nbsp; &nbsp;Talent Management</li>
	<li>o&nbsp;&nbsp; &nbsp;Visibility</li>
	<li>o&nbsp;&nbsp; &nbsp;Adaptation and Learning</li>
</ul>

<h3>&nbsp;Strategic Priority areas</h3>

<ul>
	<li>o&nbsp;&nbsp; &nbsp;A stronger and influential ECCSA;</li>
	<li>o&nbsp;&nbsp; &nbsp;Be a positive influence on the creation of business friendly environment;</li>
	<li>o&nbsp;&nbsp; &nbsp;The leading voice of the business community in the country;</li>
	<li>o&nbsp;&nbsp; &nbsp;Create a stronger and influential Chambers/Associations;</li>
	<li>o&nbsp;&nbsp; &nbsp;Raise the private sector profile.</li>
</ul>

<p>ECCSA is responsible to complete its mission of trade and investment promotion, capacity building and research &amp; advocacy. To do these, it has been reorganized in to four directorates:-Research and Advocacy Directorate, Investment and Trade Promotion Directorate, Communication and Public Relation Directorate, and Organizational Support Directorate. These directorates have been undertaking the following basic roles and responsibilities:</p>

<h3>Promote Product and Services</h3>

<p>The Chamber offers numerous opportunities to enhance your business visibility through target marketing and event showcases:&nbsp;</p>

<ul>
	<li>o&nbsp;&nbsp; &nbsp;Local and foreign Market opportunities</li>
	<li>o&nbsp;&nbsp; &nbsp;Event sponsorship</li>
	<li>o&nbsp;&nbsp; &nbsp;Business Directory</li>
	<li>o&nbsp;&nbsp; &nbsp;Sharing members tangible experiences</li>
</ul>

<h3>Create Better Business Networks</h3>
The chamber builds your social responsibility by connecting you to other businesses developing relationships and helping you gain visibility in the international market system.
<ul>
<li>o&nbsp;&nbsp; &nbsp;Provision of business information to the business community</li>
<li>o&nbsp;&nbsp; &nbsp;Business advisory services to issues related to Trade and Industry expansion</li>
<li>o&nbsp;&nbsp; &nbsp;Business networking</li>
<li>o&nbsp;&nbsp; &nbsp;Trade Fairs/Exhibition</li>
<li>o&nbsp;&nbsp; &nbsp;Investment Guide</li>
<li>o&nbsp;&nbsp; &nbsp;Exporters/importer Guide</li>
<li>o&nbsp;&nbsp; &nbsp;Issuance of Certificate of Origin</li>
<li>o&nbsp;&nbsp; &nbsp;Document Authentication (Export)</li>
</ul>
<h3>&nbsp;&nbsp;&nbsp; &nbsp;Build Your Business Skills</h3>
&nbsp;<br />
The chamber provides opportunities to you and your employees to connect with other business leaders enhance your skills and engage with the community.<br />
<ul>
<li>&bull;&nbsp;&nbsp; &nbsp;Need based training</li>
<li>&bull;&nbsp;&nbsp; &nbsp;Technical and Skill development &nbsp;trainings</li>
<li>&bull;&nbsp;&nbsp; &nbsp;Business education seminars and conferences</li>
<li>&bull;&nbsp;&nbsp; &nbsp;Soliciting supports for members from donors&nbsp;</li>
<li>&bull;&nbsp;&nbsp; &nbsp;Capacity building programs</li>
</ul>
<h3>&nbsp;&nbsp;&nbsp; &nbsp;Give You a Voice</h3>

<p>&nbsp;The chamber provides your business with the opportunities&nbsp;</p>

<ul>
	<li>o&nbsp;&nbsp; &nbsp;Research &amp; Policy Advocacy&nbsp;</li>
	<li>o&nbsp;&nbsp; &nbsp;Publication</li>
	<li>o&nbsp;&nbsp; &nbsp;Radio Program</li>
	<li>o&nbsp;&nbsp; &nbsp;Social Media</li>
</ul>

<h3>Legal Counsel &amp; Arbitration</h3>

<ul>
	<li>o&nbsp;&nbsp; &nbsp;Legal Counseling&nbsp;</li>
	<li>o&nbsp;&nbsp; &nbsp;Arbitration Service</li>
</ul>

<h3>Information</h3>

<ul>
	<li>o&nbsp;&nbsp; &nbsp;Gives information about International events</li>
	<li>o&nbsp;&nbsp; &nbsp;Works in collaboration with government and serves as a bridge for the private sector</li>
	<li>o&nbsp;&nbsp; &nbsp;Business linkage for institutions, Associations and for those involved in Trade and investment locally and abroad</li>
</ul>

<h3><br />
ECCSA&rsquo;s Members Telephone number</h3>

<p>Afar Chamber of Commerce and Sectoral Associations (Tel. +251-913-30-79-99)<br />
Amhara Chamber of Commerce and Sectoral Associations (Tel. +251-582-22-17-30/+251-582-26-40-52)<br />
Benishangul Chamber of Commerce and Sectoral Associations (Tel.+251-578-75-91-64/+251-578-75-01-06)<br />
Diredawa Chamber of Commerce and Sectoral Associations (Tel. +251-251-11-3082)<br />
Gambella Chamber of Commerce and Sectoral Associations (Tel. +251-475-51-00-69)<br />
Harari Chamber of Commerce and Sectoral Associations (Tel. +251-258-66-91-09)<br />
Oromia Chamber of Commerce and Sectoral Associations (Tel. +251-115-50-98-39/+251-115-54-92-71)<br />
Somali Chamber of Commerce and Sectoral Associations (Tel. +251-257-75-73-92)<br />
South Chamber of Commerce and Sectoral Associations (Tel. +251-462-20-88-98)<br />
Tigray Chamber of Commerce and Sectoral Associations (Tel. +251-344-40-17-60)<br />
Addis Ababa Chamber of Commerce and Sectoral Associations (Tel. +251-115-51-80-55/+251-115-51-38-14)<br />
Ethiopian Chamber of Sectoral Associations (Tel. +251-115-54-21-44/+251-115-52-01-34)<br />
Ethiopian Horticulture (Flowers) produce exporters Associations (Tel. +251-116-63-67-50)<br />
Ethiopian Leather Associations (Tel. +251-115-15-61-44)<br />
Ethiopian Textile Associations (Tel. +251-115-57-60-02)<br />
Ethiopian Meat exporters Associations (Tel. +251-113-72-88-45)<br />
Ethiopian Pulses oil Seeds &amp; spice processers&rsquo; exporters Associations (Tel. +251-116-62-35-45)<br />
Ethiopian Sugar &amp; Sweet Producers Associations (Tel. +251-118-33-32-92)</p>




  </div>
                </div>
              </section>
            </div>
          </Paper>
        </Grid>
        <Grid item xs={12} md={3}>
          <QuickLinks />
        </Grid>
      </Grid>
    </div>
  );
}

export default App;
