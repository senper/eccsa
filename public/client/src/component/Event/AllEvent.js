import React from 'react';
import AppBar from '@material-ui/core/AppBar';
import Button from '@material-ui/core/Button';
import CameraIcon from '@material-ui/icons/PhotoCamera';
import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import CardMedia from '@material-ui/core/CardMedia';
import CssBaseline from '@material-ui/core/CssBaseline';
import Grid from '@material-ui/core/Grid';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import { makeStyles } from '@material-ui/core/styles';
import Container from '@material-ui/core/Container';
import Link from '@material-ui/core/Link';

const useStyles = makeStyles((theme) => ({
  icon: {
    marginRight: theme.spacing(2),
  },
  heroContent: {
    backgroundColor: theme.palette.background.green,
    padding: theme.spacing(2, 0, 1),
  },
  heroButtons: {
    marginTop: theme.spacing(4),
  },
  cardGrid: {
    paddingTop: theme.spacing(2),
    paddingBottom: theme.spacing(2),
    maxWidth:'100%',
  },
  card: {
    height: '100%',
    display: 'flex',
    flexDirection: 'column',
  },
  cardMedia: {
    paddingTop: '56.25%', // 16:9
  },
  cardContent: {
    flexGrow: 1,
  },
  footer: {
    backgroundColor:'#fff',
    padding: theme.spacing(1),
  },
  title:{
    color: '#198754',
    // textAlign: 'center',
    // fontFamily: 'AvenirBlack',
    fontWeight: '500',
    fontStyle: 'normal',
    textAlign:'center',
    // color: '#aaa',
    // background: '#198754',

  },
  paper:{ 
    padding:theme.spacing(1),
    textAlign:'center',
    color: '#000',

    background:'#fff',
  },
}));

// const cards = [1, 2, 3, 4];

export default function Album() {
  const classes = useStyles();

  return (
    <React.Fragment>
      <CssBaseline />
      
      <main>
        {/* Hero unit */}
        {/* <div className={classes.heroContent}>
       
          
        </div> */}
        <Container className={classes.cardGrid} >
          {/* End hero unit */}
          <Grid container spacing={1}>
            {/* {cards.map((card) => ( */}
              <Grid item  xs={12} sm={6} md={3}>
                <Card className={classes.card}>
                  <CardMedia
                    className={classes.cardMedia}
                    image="https://ethiopianmonitor.com/wp-content/uploads/2021/06/198815882_3050247588532143_2746811636147079007_n-e1623612525297-800x445.jpg"
                    title="Image title"
                  />
                  <CardContent className={classes.cardContent}>
                    <Typography gutterBottom variant="h6" className={classes.title}>
                    New Year Exhibition                </Typography>
                    
                   Ethioppian New Year Exhibition </CardContent>
                  <CardActions>
                    
                    <Button variant="outlined" href="#" class="btn btn-outline-success" color="primary">
                   Read more
                  </Button>
                  </CardActions>
                </Card>
                
              </Grid>
              <Grid item  xs={12} sm={6} md={3}>
              <Card className={classes.card}>
                  <CardMedia
                    className={classes.cardMedia}
                    image="https://ethiopianmonitor.com/wp-content/uploads/2021/06/198815882_3050247588532143_2746811636147079007_n-e1623612525297-800x445.jpg"
                    title="Image title"
                  />
                  <CardContent className={classes.cardContent}>
                    <Typography gutterBottom variant="h6" className={classes.title}>
                    New Year Exhibition                </Typography>
                    
                   Ethioppian New Year Exhibition </CardContent>
                  <CardActions>
                    
                    <Button variant="outlined" href="#" class="btn btn-outline-success" color="primary">
                   Read more
                  </Button>
                  </CardActions>
                </Card>
                
              </Grid>
              <Grid item  xs={12} sm={6} md={3}>
              <Card className={classes.card}>
                  <CardMedia
                    className={classes.cardMedia}
                    image="https://ethiopianmonitor.com/wp-content/uploads/2021/06/198815882_3050247588532143_2746811636147079007_n-e1623612525297-800x445.jpg"
                    title="Image title"
                  />
                  <CardContent className={classes.cardContent}>
                    <Typography gutterBottom variant="h6" className={classes.title}>
                    New Year Exhibition                </Typography>
                    
                   Ethioppian New Year Exhibition </CardContent>
                  <CardActions>
                    
                    <Button variant="outlined" href="#" class="btn btn-outline-success" color="primary">
                   Read more
                  </Button>
                  </CardActions>
                </Card>
                
              </Grid>
              
          </Grid>
        </Container>
      </main>
      {/* Footer */}
      
      {/* End footer */}
    </React.Fragment>
  );
}