import React from 'react';
import { makeStyles } from "@material-ui/core/styles";
import { Grid,Paper } from "@material-ui/core";
import 'bootstrap/dist/css/bootstrap.min.css';
 import '../App.css';
import '../component/slider/HomeSlider';

import 'bootstrap/dist/css/bootstrap.css';

import Footer from '../component/Footer/Footer';


const useStyle =makeStyles((theme)=>({
  grid:{
    width:'100%',
    margin:'0px',
    padding:'3px'
  },
  paper:{ 
    padding:theme.spacing(2),
    textAlign:'center',
    color: theme.palette.text.secondary,
    background:theme.palette.success.light,
  }
}))

function App() {
  const classes=useStyle();
  return (
    <div class="mmenu">
    
     <Grid container spacing={2} className="grid">
     <Grid item xs={1} md={1}>
        
        {/* <Welcome /> */}
        
        </Grid>
        <Grid item xs={11} md={8}>
         
           <h3>	Structure of the Chamber System</h3>
  
 <img src="./images/structure.jpg" className="responsive">
 </img>


        </Grid>
        <Grid item xs={12} md={3}>
          <Paper >
          {/* <ServiceLinks /> */}
          </Paper>
        </Grid>
        
     </Grid>
    </div>
  );
}

export default App;
