import React from 'react';

import { makeStyles } from "@material-ui/core/styles";
import { Grid, Paper } from "@material-ui/core";
import { Link } from 'react-router-dom';
import Breadcrumbs from '@material-ui/core/Breadcrumbs';
import 'bootstrap/dist/css/bootstrap.min.css';
import '../App.css';
import './Pages.css';
import '../component/slider/HomeSlider';
import 'bootstrap/dist/css/bootstrap.css';
import { Tabs, Tab } from "react-bootstrap";

const useStyle = makeStyles((theme) => ({
  grid: {
    width: '100%',
    margin: '0px',
    padding: '3px'
  },
  main: {
    width: '100%',
    background: '#fff',
    padding: '10px',
  },
  breadcrum: {
    backgroundColor: '#fff',
    padding: '10px',
    marginLeft: '100px',
  }
}));
const cardStyle = {
};
function handleClick(event) {
  event.preventDefault();

}
function FormerPresident({ presidents, secretaries }) {
  const classes = useStyle();
  return (
    <div class="mmenu root">
      <div className={classes.main}>
        <Grid container spacing={2} className="grid">
          <Grid item xs={12} md={12}>
            <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrum}>
              <Link color="inherit" to="/" >
                Home
              </Link>
              <Link
                color="black"
                to="#"
               
                aria-current="page"
              >
                Former Presidents & Secretary G.
              </Link>
            </Breadcrumbs>
          </Grid>
          <Grid item xs={12} md={12}>
            <div class="container">
              <Tabs defaultActiveKey="former" id="122" className="mb-3">
                <Tab eventKey="former" title="Former President">
                  <div className="row">
                    {presidents.map((row, index) => (
                      <div class="col-sm-2" style={{ marginBottom: '10px' }}>
                        <div class="card" >
                          <div class="card-body">
                            <img src={row.imageUrl} alt="" style={{ width: '100%' }} />
                          </div>
                        </div>
                      </div>))}
                  </div>
                </Tab>
                <Tab eventKey="secretary" title="Former Secretary General">
                  <div className="row">
                    {secretaries.map((row, index) => (
                      <div class="col-sm-2" style={{ marginBottom: '10px' }}>
                        <div class="card" >
                          <div class="card-body">
                            <img src={row.imageUrl} alt="" style={{ width: '100%' }} />
                          </div>
                        </div>
                      </div>))}
                  </div>
                </Tab>
              </Tabs>
            </div>
          </Grid>
        </Grid>
      </div>
    </div >
  );
}
export default FormerPresident;
