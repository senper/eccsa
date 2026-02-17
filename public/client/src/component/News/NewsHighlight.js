import React from "react";
import Card from "@material-ui/core/Card";
import CardActions from "@material-ui/core/CardActions";
import CardContent from "@material-ui/core/CardContent";
import CardMedia from "@material-ui/core/CardMedia";
import Grid from "@material-ui/core/Grid";
import Typography from "@material-ui/core/Typography";
import { makeStyles } from "@material-ui/core/styles";
import Container from "@material-ui/core/Container";
import { Link } from "react-router-dom";
import { getNewsHighlight } from "../../services/news";

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
    maxWidth: "100%",
  },
  card: {
    height: "100%",
    display: "flex",
    flexDirection: "column",
  },
  cardMedia: {
    paddingTop: "56.25%",
  },
  cardContent: {
    flexGrow: 1,
  },
  footer: {
    backgroundColor: "#fff",
    padding: theme.spacing(1),
  },
  title: {
    color: "#198754",
    fontWeight: "500",
    fontStyle: "normal",
    textAlign: "center",
  },
  paper: {
    padding: theme.spacing(1),
    textAlign: "center",
    color: "#000",

    background: "#fff",
  },
}));

// const cards = [1, 2, 3, 4];

export default function Album() {
  const classes = useStyles();
  const [news, setNews] = React.useState([]);
  React.useEffect(() => {
    getNewsHighlight().then((data) => {
      setNews(data.data.$values);
    });
  }, []);
  const imgurl = (id, extension) => {
    const idStr = id.toString();
    const name = idStr.substr(idStr.length - 15).replace("-", "");
    return `${process.env.REACT_APP_API_URL}/pr/documents/${name}${extension}`;
  };
  return (
    <>
      <main>
        <Container className={classes.cardGrid}>
          {/* End hero unit */}
          <Grid container spacing={1}>
            {/* {cards.map((card) => ( */}
            {news
              ? news.map((newsValue) => (
                  <Grid item xs={12} sm={6} md={3} key={newsValue.partner_id}>
                    <Card className={classes.card}>
                      {newsValue && newsValue.bannerSetted ? (
                        <CardMedia
                          className={classes.cardMedia}
                          image={imgurl(newsValue.id, newsValue.extension)}
                          title={newsValue.title}
                        />
                      ) : null}
                      <CardContent className={classes.cardContent}>
                        <Typography
                          gutterBottom
                          variant="h6"
                          className={classes.title}
                        >
                          {newsValue.title}
                        </Typography>
                        <Typography class="body">{newsValue.brief}</Typography>
                      </CardContent>
                      <CardActions>
                        <Link
                          class="btn btn-outline-success"
                          eventKey="news"
                          to={{
                            pathname: "/news_detail",
                            state: { newsValue, eventKey: "news" },
                          }}
                        >
                          Read More{" "}
                        </Link>
                      </CardActions>
                    </Card>
                  </Grid>
                ))
              : null}
            <Grid item xs={12} md={6}></Grid>
            <Grid item xs={12} md={6}></Grid>
          </Grid>
        </Container>
      </main>
    </>
  );
}
