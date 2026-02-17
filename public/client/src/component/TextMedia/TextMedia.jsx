import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import "bootstrap/dist/css/bootstrap.min.css";
import "../slider/HomeSlider";
import "bootstrap/dist/css/bootstrap.css";
import Footer from "../Footer/Footer";
import { Breadcrumbs, Grid } from "@material-ui/core";
import { getTextMediaList, textMediaTypes } from "../../services/otherMedia";
import { Link, useLocation } from "react-router-dom";
import Card from "@material-ui/core/Card";
import CardActions from "@material-ui/core/CardActions";
import CardContent from "@material-ui/core/CardContent";
import Typography from "@material-ui/core/Typography";
import Container from "@material-ui/core/Container";
import { FaEye } from "react-icons/fa";

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
  },
  cardGrid: {
    paddingTop: theme.spacing(2),
    paddingBottom: theme.spacing(2),
    maxWidth: "100%",
  },
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
  iconStyle: {
    fontSize: "20px",
  },
}));

export default function TextMedia() {
  const classes = useStyle();
  const location = useLocation();
  let mediaCategoryValue = 0;
  if (location.state) {
    const { mediaCategory } = location.state;
    mediaCategoryValue = mediaCategory;
  }
  const [textMedias, setTextMedias] = React.useState([]);
  React.useEffect(() => {
    if (mediaCategoryValue) {
      getTextMediaList(mediaCategoryValue).then((data) => {
        setTextMedias(data.data.$values);
      });
    }
  }, [mediaCategoryValue]);

  const viewsText = (media) =>
    media.views > 999 ? media.views / 1000 : media.views;
  const displayMedias = textMedias.map((value) => {
    return (
      <Grid item xs={12} sm={6} md={3} key={value.id}>
        <Card className={classes.card}>
          <CardContent className={classes.cardContent}>
            <Typography gutterBottom variant="h6" className={classes.title}>
              {value.title}{" "}
            </Typography>
            <Typography class="body">{value.brief}</Typography>
          </CardContent>
          <CardActions>
            <Link
              class="btn btn-outline-success"
              eventKey="news"
              style={{ margin: "0" }}
              to={{
                pathname: "/text_media_detail",
                state: { mediaCategory: mediaCategoryValue, mediaId: value.id },
              }}
            >
              Read More{" "}
            </Link>
            <span >
              <FaEye
                className={classes.iconStyle}
                title={"Views Count " + viewsText(value)}
              />{" "}
              {viewsText(value)}
            </span>
          </CardActions>
        </Card>
      </Grid>
    );
  });
  function getText() {
    return mediaCategoryValue
      ? textMediaTypes.find((s) => s.value == mediaCategoryValue).text
      : null;
  }
  return (
    <div class="mmenu">
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
              <Link color="textPrimary" to="/news" aria-current="page">
                {getText()}
              </Link>
            </Breadcrumbs>
          </Grid>
          <br />
          <Grid container spacing={1}>
            {textMedias && textMedias.length > 0 ? (
              displayMedias
            ) : (
              <h3>No {getText()} Available</h3>
            )}
          </Grid>
        </Container>
      </main>

    </div>
  );
}
