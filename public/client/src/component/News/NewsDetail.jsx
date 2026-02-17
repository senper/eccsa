//src/pages/News2.jsx

import React from "react";

import { makeStyles } from "@material-ui/core/styles";
import {
  Grid,
  Card,
  Typography,
  cardContent,
  CardContent,
  Breadcrumbs,
} from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.min.css";
import "../slider/HomeSlider";
import "bootstrap/dist/css/bootstrap.css";
import Footer from "../Footer/Footer";
import { Link, useLocation } from "react-router-dom";

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
  title: {
    color: "#198754",
    fontWeight: "500",
    fontStyle: "normal",
    textAlign: "left",
  },
}));

export default function NewsDetail(props) {
  window.scrollTo({
    top: 0,
    behavior: "smooth",
  });
  const classes = useStyle();
  const [news, setNews] = React.useState(null);
  const location = useLocation();
  let newsParam = null;
  if (location.state && location.state.newsValue) {
    const { newsValue } = location.state;
    newsParam = newsValue;
  }
  React.useEffect(() => {
    if (newsParam) {
      setNews(newsParam);
    }
  }, [newsParam]);
  const formateProfile = () => {
    let profile = news.full;
    profile = profile.replace(
      'src="/pr/news',
      `src="${process.env.REACT_APP_API_URL}/pr/news`
    );
    return profile;
  };
  return (
    <div class="mmenu">
      <Grid container spacing={2} className="grid">
        <Grid item xs={12} md={12}>
          <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrum}>
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
            <Link color="" aria-current="page">
              New Detail
            </Link>
          </Breadcrumbs>
        </Grid>
        <Grid item xs={1} md={1}></Grid>
        <Grid item xs={11} md={12}>
          <Grid item xs={12} sm={6} md={12}>
            {news ? (
              <Card className={classes.card}>
                <Typography gutterBottom variant="h6" className={classes.title}>
                  {news.title}
                </Typography>
                <CardContent className={classes.cardContent}>
                  <Typography class="body">
                    {news && news.bannerSetted ? (
                      <img
                        src={`${process.env.REACT_APP_API_URL}\\${news.bannerUrlRelative}`}
                        width="65%"
                        style={{ maxWidth: "900px" }}
                        alt={news.title}
                      />
                    ) : null}
                    <div
                      dangerouslySetInnerHTML={{ __html: formateProfile() }}
                    />
                    <h7>Author:{news.author}</h7>
                    <br></br>
                    <h7>{news.newsDate.substr(0, 10)}</h7>
                  </Typography>
                </CardContent>
              </Card>
            ) : null}
          </Grid>
        </Grid>
        
      </Grid>
    </div>
  );
}
