import React from "react";

import { makeStyles } from "@material-ui/core/styles";
import { Grid } from "@material-ui/core";
import { Link } from "react-router-dom";
import Breadcrumbs from "@material-ui/core/Breadcrumbs";
import "bootstrap/dist/css/bootstrap.min.css";
import "../../App.css";
import "../../pages/Pages.css";
import "bootstrap/dist/css/bootstrap.css";
import "react-image-gallery/styles/css/image-gallery.css";
import YouTube from "react-youtube-embed";
import { Card } from "react-bootstrap";
import { getVideoGalleries } from "../../services/gallery";
import Footer from "../Footer/Footer";

const useStyle = makeStyles((theme) => ({
  grid: {
    width: "100%",
    margin: "0px",
    padding: "3px",
  },
  main: {
    width: "100%",
    background: "#fff",
    padding: "10px",
  },
  breadcrum: {
    backgroundColor: "#fff",
    padding: "10px",
    marginLeft: "100px",
  },
}));
export default function VideoGallery() {
  const classes = useStyle();
  const [loading, setLoading] = React.useState(false);
  const [videos, setVideos] = React.useState([]);
  React.useEffect(() => {
    setLoading(true);
    getVideoGalleries()
      .then((data) => {
        setVideos(data.data.$values);
        setLoading(false);
      })
      .catch((error) => {
        setLoading(false);
      });
  }, []);
  return (
    <div class="mmenu root">
      <div className={classes.main}>
        <Grid container spacing={2} className="grid">
          <Grid item xs={12} md={12}>
            <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrum}>
              <Link color="inherit" to="/">
                Home
              </Link>
              <Link color="black" to="#" aria-current="page">
                Gallery
              </Link>
              <Link color="black" to="#" aria-current="page">
                Video
              </Link>
            </Breadcrumbs>
          </Grid>
          <Grid item xs={12} md={12}>
            <div class="row">
              {loading ? <h3>Loading...</h3> : null}
              {videos
                ? videos.map((row, index) => (
                    <div class="col-sm-4" style={{ marginBottom: "10px" }}>
                      <YouTube id={row.url} />
                      <Card>
                        <Card.Title>{row.title}</Card.Title>
                        <Card.Text>{row.description}</Card.Text>
                      </Card>
                    </div>
                  ))
                : null}
              {!loading && (!videos || videos.length <= 0) ? (
                <h3>NOTHING FOUND</h3>
              ) : null}
            </div>
          </Grid>
        </Grid>
      </div>
    </div>
  );
}
