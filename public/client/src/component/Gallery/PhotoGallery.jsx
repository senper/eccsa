import React from "react";

import { makeStyles } from "@material-ui/core/styles";
import { Grid } from "@material-ui/core";
import { Link } from "react-router-dom";
import Breadcrumbs from "@material-ui/core/Breadcrumbs";
import "bootstrap/dist/css/bootstrap.min.css";
import "../../App.css";
import "../../pages/Pages.css";
import "bootstrap/dist/css/bootstrap.css";
import { Card } from "react-bootstrap";
import Footer from "../Footer/Footer";
import { getPhotoAlbums } from "../../services/gallery";

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

export default function PhotoGallery() {
  const classes = useStyle();
  const [loading, setLoading] = React.useState(false);
  const [albums, setAlbums] = React.useState([]);
  React.useEffect(() => {
    setLoading(true);
    getPhotoAlbums()
      .then((data) => {
        setAlbums(data.data.$values);
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
                Media
              </Link>
              <Link color="black" to="#" aria-current="page">
                Albums
              </Link>
            </Breadcrumbs>
          </Grid>
          <Grid item xs={12} md={12}>
            <div class="row">
              {loading ? <h3>Loading...</h3> : null}
              {albums
                ? albums.map((row, index) => (
                    <div class="col-sm-3" style={{ marginBottom: "10px" }}>
                      <Card>
                        <Card.Img
                          variant="top"
                          src={row.imageurl}
                          style={{ width: "20px" }}
                        />
                        <Card.Body>
                          <Card.Title>{row.title}</Card.Title>
                          <Card.Text>{row.description}</Card.Text>
                          <Link
                            to={{
                              pathname: "/pictureView",
                              state: {
                                albumId: row.id,
                                eventKey: "picture",
                              },
                            }}
                          >
                            View
                          </Link>
                        </Card.Body>
                        {/* <Card.Footer>
                      <small className="text-muted">
                        Uploaded date: {row.created}{" "}
                      </small>
                    </Card.Footer> */}
                      </Card>
                    </div>
                  ))
                : null}
              {!loading && (!albums || albums.length <= 0) ? (
                <h3>NOTHING FOUND</h3>
              ) : null}
            </div>
          </Grid>
        </Grid>
      </div>
    </div>
  );
}
