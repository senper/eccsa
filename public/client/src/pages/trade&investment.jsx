import React from 'react';

import { makeStyles } from "@material-ui/core/styles";
import { Grid, Paper } from "@material-ui/core";
import 'bootstrap/dist/css/bootstrap.min.css';
import '../App.css';
import '../component/slider/HomeSlider';
import {  Nav, Tab, Card, Row, Col, Form } from "react-bootstrap";
import { Breadcrumbs } from '@material-ui/core';
import Footer from '../component/Footer/Footer';
import { Link } from 'react-router-dom';

const useStyle = makeStyles((theme) => ({
  grid: {
    width: '100%',
    margin: '0px',
    padding: '3px',
    background: '#fff'
  },
  paper: {
    padding: theme.spacing(2),
    textAlign: 'center',
    color: theme.palette.text.secondary,
    // background:theme.palette.success.light,
  },
  breadcrum: {
    backgroundColor: '#fff',
    padding: '10px',
    marginLeft: '100px',
       }
}))

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
     <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrum} >
                <Link color="inherit" to="/" >
              Home
            </Link>

            <Link
              color="textPrimary"
              to="#"
              
              aria-current="page">
              Trade & Investment Promotion
            </Link>
            </Breadcrumbs>
       </Grid>
       {/* <div className="app2"> */}
       <Grid item xs={1} md={1}>
       
</Grid>
       
      
        <Grid item xs={11} md={10}>
         
        <Paper >
        <h5 class="section-title h3">Trade & Investment Promotion</h5>
        
          <Tab.Container id="left-tabs-example" defaultActiveKey="1">
          <Row>
          <Col sm={3}>
            <Nav variant="pills" className="">
            <Nav.Item>
                <Nav.Link eventKey="1">Commercial Invoice Approval</Nav.Link>
              </Nav.Item>
              <Nav.Item>
                <Nav.Link eventKey="2" >Approval of packing list</Nav.Link>
              </Nav.Item>
              <Nav.Item>
                <Nav.Link eventKey="3">Approval of Health Certificate</Nav.Link>
              </Nav.Item>
              <Nav.Item>
                <Nav.Link eventKey="4">Request for support letter to Embassy/ MOFA</Nav.Link>
              </Nav.Item>
              <Nav.Item>
                <Nav.Link eventKey="5">Free sales certificate approval</Nav.Link>
              </Nav.Item>
              <Nav.Item>
                <Nav.Link eventKey="6">Track Waybill Approval</Nav.Link>
              </Nav.Item>
              <Nav.Item>
                <Nav.Link eventKey="7">Approval of Proforma Invoice</Nav.Link>
              </Nav.Item>
             
              <Nav.Item>
                <Nav.Link eventKey="8">Issuance of Certificate of Origin</Nav.Link>
              </Nav.Item>
              <Nav.Item>
                <Nav.Link eventKey="11">Business Networking</Nav.Link>
              </Nav.Item>
              <Nav.Item>
                <Nav.Link eventKey="12" >Business Advisory</Nav.Link>
              </Nav.Item>
              <Nav.Item>
                <Nav.Link eventKey="13">Trade Fairs/Exhibition</Nav.Link>
              </Nav.Item>
              <Nav.Item>
                <Nav.Link eventKey="14">Investment Guide</Nav.Link>
              </Nav.Item>
              <Nav.Item>
                <Nav.Link eventKey="15">Exporters/Importer Guide</Nav.Link>
              </Nav.Item>
              
              <Nav.Item>
                <Nav.Link eventKey="17">Issuance of Certificate of Origin</Nav.Link>
              </Nav.Item>
              <Nav.Item>
                <Nav.Link eventKey="18">Trade Promotion</Nav.Link>
              </Nav.Item>
              <Nav.Item>
                <Nav.Link eventKey="19">Legal Counselling</Nav.Link>
              </Nav.Item>
             
            </Nav>
          </Col>
          <Col sm={9}>
            <Tab.Content>
            <Tab.Pane eventKey="1">
            Prerequisites: attaching the following documents 
            <ol><li> Trade License</li><li> Bank Permit with Bank Stamp</li><li> CoO</li><li> Bill of Loading/Air way bill
            </li></ol>
            <Link  eventKey="service" to="/login" class="btn btn-success"> Request Service</Link>
            
              </Tab.Pane>
              <Tab.Pane eventKey="2">
              Prerequisites: attaching the following documents 
            <ol><li> Trade License</li><li> Bill of Loading/Air way bill
            </li>
            <li>Commercial Invoice</li></ol><Link  eventKey="service" to="/login" class="btn btn-success"> Request Service</Link>
              </Tab.Pane>
              <Tab.Pane eventKey="3">
              Prerequisites: attaching the following documents 
            <ol><li> Trade License</li><li> copy of certificate signed by government organization – MOH</li></ol>
            <Link  eventKey="service" to="/login" class="btn btn-success"> Request Service</Link>
              </Tab.Pane>
              <Tab.Pane eventKey="4">
              Prerequisites: attaching the following documents 
            <ol><li> Trade License</li><li> Request-letter</li><li> Invitation-letter of Foreign Company</li><li> Renewed Chamber Membership Card</li><li> six-months-payroll for employees</li><li> support-letter-from company
              </li></ol>
              <Link  eventKey="service" to="/login" class="btn btn-success"> Request Service</Link>
              </Tab.Pane>
              <Tab.Pane eventKey="5">
              Prerequisites: attaching the following documents 
            <ol><li> Renewed Trade License</li><li> Certificates-signed by relevant-gov-organizations</li></ol>
            <Link  eventKey="service" to="/login" class="btn btn-success"> Request Service</Link>
              </Tab.Pane>
              <Tab.Pane eventKey="6">
              Prerequisites: attaching the following documents 
            <ol><li> Attachment (approval from other governmental organizations) </li></ol>
            <Link  eventKey="service" to="/login" class="btn btn-success"> Request Service</Link>
              </Tab.Pane>
              <Tab.Pane eventKey="7">
              Prerequisites: attaching the following documents 
            <ol><li> Renewed Trade License</li><li> Copy of documents</li><li> Sales Contract</li></ol>
            <Link  eventKey="service" to="/login" class="btn btn-success"> Request Service</Link>
              </Tab.Pane>
              <Tab.Pane eventKey="12">
              <h4>Business Advisory</h4>
              </Tab.Pane>
              <Tab.Pane eventKey="11">
             <h4> Business Networking</h4><p>

In every part of the world building reliable and credible business networks is not an easy task. This is why the Chamber steps in to fill such gaps for the business communities so as to help them grow by creating business links with potential partners to bolster mutual benefits.
</p>

<p>Documents to be submitted along with application for Procurement of Certificate of Origin</p>
<ol class="list-style" >
          <li>
A covering letter (on Original Letterhead) addressed to the Director General, Indian Chamber of Commerce, for issue of a Certificate of Origin / Certification of copies of Invoices / Packing List etc.
</li><li>In case of submission by Authorized Signatory attested copy of authorization letter must be attached.
</li><li>Completed Certificate of Origin Form Plus (one additional copy for our records). Blank Certificate of Origin Forms must be purchased from ICC at a cost of Rs 70/- per set of 50 pc.
</li><li>Along with Invoice, Packing List, Letter of Credit or Purchase Order or E-mail order (whichever is relevant) all the documents must be stamped and signed by the Exporter Applicant.
</li><li>In case of goods which have been imported prior to being exported to another country, relevant clearance certificates from Customs Dept must be submitted to show that the goods entered the country legally after payment of all necessary taxes and duties.
</li><li>The exporter must submit an additional set of documents for ICC records with each application for Certificate of Origin.
</li>
           </ol>   
           <Link  eventKey="service" to="/login" class="btn btn-success"> Request Service</Link>
           </Tab.Pane>
              <Tab.Pane eventKey="13">
              Trade Fairs/Exhibition
              <br/><br/>
              <Link  eventKey="service" to="/login" class="btn btn-success"> Request Service</Link>
              </Tab.Pane>
              <Tab.Pane eventKey="14">
                <h4> Investment Guide</h4><p>

In every part of the world building reliable and credible business networks is not an easy task. This is why the Chamber steps in to fill such gaps for the business communities so as to help them grow by creating business links with potential partners to bolster mutual benefits.
</p>

<p>Documents to be submitted along with application for Procurement of Certificate of Origin</p>
<ol class="list-style" >
          <li>
A covering letter (on Original Letterhead) addressed to the Director General, Indian Chamber of Commerce, for issue of a Certificate of Origin / Certification of copies of Invoices / Packing List etc.
</li><li>In case of submission by Authorized Signatory attested copy of authorization letter must be attached.
</li><li>Completed Certificate of Origin Form Plus (one additional copy for our records). Blank Certificate of Origin Forms must be purchased from ICC at a cost of Rs 70/- per set of 50 pc.
</li><li>Along with Invoice, Packing List, Letter of Credit or Purchase Order or E-mail order (whichever is relevant) all the documents must be stamped and signed by the Exporter Applicant.
</li><li>In case of goods which have been imported prior to being exported to another country, relevant clearance certificates from Customs Dept must be submitted to show that the goods entered the country legally after payment of all necessary taxes and duties.
</li><li>The exporter must submit an additional set of documents for ICC records with each application for Certificate of Origin.
</li>
           </ol>   
           <Link  eventKey="service" to="/login" class="btn btn-success"> Request Service</Link>
              </Tab.Pane>
              <Tab.Pane eventKey="15">
              Service two
              </Tab.Pane>
              <Tab.Pane eventKey="16">
              Service two
              </Tab.Pane>
              <Tab.Pane eventKey="17">
              <h4>Certificate of Origin </h4>
    <ol><li>Stamp paper should be purchased in the name of the Company/Establishment.
    </li><li>The Bond shall only be signed by the Director/Partner/Proprietor of the Company. After due process, it will be entered in the ICC records and the exporter shall be given a registration number by the ICC.
    </li><li>Bond should be required to be submitted with the profile of the Company/Establishment, PAN, I/EC Registration document, Signatures of those authorized to sign the Certificates;
    </li><li>After registration at ICC on every occasion of submission of application for Certificate of Origin the Registration Number with ICC must be mentioned for reference.
    </li> <Link  eventKey="service" to="/login" class="btn btn-success"> Request Service</Link>
         </ol>     </Tab.Pane>
            </Tab.Content>
          </Col>
        </Row>
      </Tab.Container>
          </Paper>
        </Grid>
        <Grid item xs={1} md={1}>
          </Grid>
     </Grid>
    </div>
  );
}

export default App;
