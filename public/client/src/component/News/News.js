import React from "react";
import "./Pagination.css";
import Card from "@material-ui/core/Card";
import CardActions from "@material-ui/core/CardActions";
import CardContent from "@material-ui/core/CardContent";
import CardMedia from "@material-ui/core/CardMedia";
import Grid from "@material-ui/core/Grid";
import Typography from "@material-ui/core/Typography";
import { makeStyles } from "@material-ui/core/styles";
import Container from "@material-ui/core/Container";
import { Link } from "react-router-dom";
import { getNews } from "../../services/news";
import { Breadcrumbs } from "@material-ui/core";

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

export default function Album() {
  const classes = useStyles();
  const [loading, setLoading] = React.useState(false);
  const [news, setNews] = React.useState([]);
  React.useEffect(() => {
    setLoading(true);
    getNews()
      .then((data) => {
        setNews(data.data.$values);
        setLoading(false);
      })
      .catch((error) => {
        setLoading(false);
      });
  }, []);

  const imgurl = (id, extension) => {
    const idStr = id.toString();
    const name = idStr.substr(idStr.length - 15).replace("-", "");
    return `${process.env.REACT_APP_API_URL}/pr/documents/${name}${extension}`;
  };
  const displayNews = news.map((newsValue) => {
    return (
      <Grid item xs={12} sm={6} md={3} key={newsValue.id}>
        <Card className={classes.card}>
          {newsValue && newsValue.bannerSetted ? (
            <CardMedia
              className={classes.cardMedia}
              image={imgurl(newsValue.id, newsValue.extension)}
              title={newsValue.title}
            />
          ) : null}
          <CardContent className={classes.cardContent}>
            <Typography gutterBottom variant="h6" className={classes.title}>
              {newsValue.title}
            </Typography>
            <Typography class="body">{newsValue.brief}</Typography>
          </CardContent>
          <CardActions>
            <Link
              class="btn btn-outline-success"
              style={{ margin: "0" }}
              eventKey="news"
              to={{
                pathname: "/news_detail",
                state: { newsValue },
              }}
            >
              Read More{" "}
            </Link>
          </CardActions>
        </Card>
      </Grid>
    );
  });

  return (
    <>
      <main>
        <Container className={classes.cardGrid}>
          <Grid item xs={12} md={12}>
            <Breadcrumbs aria-label="breadcrumb">
              <Link
                color="inherit"
                to={{
                  pathname: "/",
                  state: {
                    eventKey: "default",
                  },
                }}
              >
                Home
              </Link>
              <Link
                color="textPrimary"
                to={{
                  pathname: "/news",
                  state: { eventKey: "news" },
                }}
                aria-current="page"
              >
                News
              </Link>
            </Breadcrumbs>
          </Grid>
          <br />
          <Grid container spacing={1}>
            {news && news.length > 0 ? (
              displayNews
            ) : loading ? (
              <strong>Loading...</strong>
            ) : (
              <h3>No News Available</h3>
            )}
          </Grid>
        </Container>
      </main>
    </>
  );
}
