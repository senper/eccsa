import React from "react";

import { makeStyles } from "@material-ui/core/styles";
import { Grid } from "@material-ui/core";
import { Link, useLocation } from "react-router-dom";
import ImageGallery from "react-image-gallery";
import Breadcrumbs from "@material-ui/core/Breadcrumbs";
import "bootstrap/dist/css/bootstrap.min.css";
import "../../App.css";
import "../../pages/Pages.css";
import "../slider/HomeSlider";
import "bootstrap/dist/css/bootstrap.css";
import "react-image-gallery/styles/css/image-gallery.css";
import { getAlbumPhotos } from "../../services/gallery";
import { Row } from "react-bootstrap";
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
function App() {
  const location = useLocation();
  let albumIdValue = 0;
  if (location.state) {
    const { albumId } = location.state;
    albumIdValue = albumId;
  }
  const classes = useStyle();
  const [photos, setPhotos] = React.useState([]);
  React.useEffect(() => {
    if (albumIdValue) {
      getAlbumPhotos(albumIdValue).then((data) => {
        setPhotos(data.data.$values);
      });
    }
  }, [albumIdValue]);
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
                Media
              </Link>
              <Link
                color="black"
                to={{
                  pathname: "/picture",
                  state: {
                    eventKey: "picture",
                  },
                }}
                aria-current="page"
              >
                Albums
              </Link>
              <Link color="black" to="#" aria-current="page">
                Photos
              </Link>
            </Breadcrumbs>
          </Grid>
          <Grid item xs={12} md={12}>
            <Row>
              <ImageGallery
                items={photos.map((v) => ({
                  original: process.env.REACT_APP_API_URL + "/" + v,
                  thumbnail: process.env.REACT_APP_API_URL + "/" + v,
                }))}
              />
            </Row>
          </Grid>
        </Grid>
      </div>
    </div>
  );
}
export default App;
