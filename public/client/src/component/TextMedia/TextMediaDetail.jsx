//src/pages/News2.jsx

import React from "react";

import { makeStyles } from "@material-ui/core/styles";
import {
  Grid,
  CardMedia,
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
import {
  addComment,
  getTextMedia,
  textMediaTypes,
} from "../../services/otherMedia";
import { Card, Col, Row } from "react-bootstrap";
import {
  FaEye,
  FaRegThumbsDown,
  FaRegThumbsUp,
  FaThumbsUp,
} from "react-icons/fa";

const useStyle = makeStyles((theme) => ({
  grid: {
    width: "100%",
    margin: "0px",
    padding: "3px",
  },
  card: {
    marginLeft: "10px",
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
  iconStyle: {
    fontSize: "20px",
  },
}));

export default function TextMediaDetail(props) {
  var token = localStorage.getItem("access_token");
  const classes = useStyle();
  const [media, setMedia] = React.useState(null);
  const location = useLocation();
  let mediaCategoryValue = 0;
  if (location.state && location.state.mediaCategory) {
    const { mediaCategory } = location.state;
    mediaCategoryValue = mediaCategory;
  }
  let mediaIdValue = null;
  if (location.state && location.state.mediaId) {
    const { mediaId } = location.state;
    mediaIdValue = mediaId;
  }
  React.useEffect(() => {
    if (mediaIdValue) {
      loadMedia();
    }
  }, [mediaIdValue]);
  function loadMedia() {
    getTextMedia(mediaIdValue).then((data) => {
      setMedia(data.data);
    });
  }
  function getText() {
    return mediaCategoryValue
      ? textMediaTypes.find((s) => s.value == mediaCategoryValue).text
      : null;
  }

  const [comment, setComment] = React.useState(null);
  function saveComment(event) {
    event.preventDefault();
    addComment(mediaIdValue, comment).then((data) => {
      setComment("");
      loadMedia();
    });
  }

  const formateProfile = () => {
    let profile = media.content;
    profile = profile.replace(
      'src="/pr/text_media',
      `src="${process.env.REACT_APP_API_URL}/pr/text_media`
    );
    return profile;
  };
  const viewsText = media ? ( media.views > 999 ? (media.views/1000) : media.views ) : null;
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
                pathname: "/textMedias",
                state: {
                  mediaCategory: mediaCategoryValue,
                  eventKey: getText(),
                },
              }}
              aria-current="page"
            >
              {getText()}
            </Link>
            <Link color="" aria-current="page">
              Detail
            </Link>
          </Breadcrumbs>
        </Grid>
        <Grid item xs={1} md={1}></Grid>
        <Grid item xs={11} md={12}>
          <Grid item xs={12} sm={6} md={12}>
            {media ? (
              <Card className={classes.card}>
                <Card.Header
                  style={{
                    backgroundColor: "#f5f5f5",
                    color: "black",
                    fontWeight: "bold",
                    fontSize: "16px",
                  }}
                >
                  {media.title}
                </Card.Header>
                <CardContent className={classes.cardContent}>
                  <Typography class="body">
                    <div dangerouslySetInnerHTML={{ __html: formateProfile() }} />
                    <h7>Author:{media.authorName}</h7>
                    <br></br>
                    <h7>{media.dateCreatedStr}</h7>
                  </Typography>
                  <hr
                    style={{
                      borderWidth: "10px",
                      width: "100%",
                      margin: "5px 0 5px 0",
                    }}
                  />
                  <Row>
                    <Col>
                        <FaEye className={classes.iconStyle} title={"Views Count " + viewsText}/>{" "}
                        {viewsText}
                      {/*<span style={{marginRight: "20px"}}>
                        <FaRegThumbsUp className={classes.iconStyle}
                        onClick={like}
                        /> {media.likes}</span>
                      {" "}
                      <span style={{marginRight: "20px"}}><FaRegThumbsDown className={classes.iconStyle}                      
                      onClick={disLike}/> {media.dislikes}</span>*/}
                    </Col>
                  </Row>
                </CardContent>
                <Card.Footer>
                  <Row>
                    <h3>Comments</h3>
                    {media.comments &&
                    media.comments.$values &&
                    media.comments.$values.length > 0 ? (
                      media.comments.$values.map((r, index) => (
                        <div class="card" style={{ marginBottom: "10px" }}>
                          <div class="card-body card-subtitle mb-2 text-muted">
                            {" "}
                            {r.text}
                          </div>
                          <div style={{ fontStyle: "italic" }} role="alert">
                            <p>
                              <small>Written by: {r.authorName}</small>
                            </p>
                          </div>
                        </div>
                      ))
                    ) : (
                      <strong>No Comments yet</strong>
                    )}
                  </Row>
                  {token ? (
                    <>
                      <h3>Write Your Comments</h3>
                      <form
                        onSubmit={saveComment}
                        className="comment-form form-group"
                      >
                        <div class="form-group">
                          <label for="exampleFormControlTextarea1">
                            Comment{" "}
                          </label>
                          <textarea
                            class="form-control"
                            rows="3"
                            value={comment}
                            onChange={(evt) => setComment(evt.target.value)}
                          ></textarea>
                        </div>
                        <input
                          style={{ margin: "8px 0 0 0 " }}
                          type="submit"
                          value="Save"
                          className="btn btn-primary"
                        />
                      </form>
                    </>
                  ) : null}
                </Card.Footer>
              </Card>
            ) : null}
          </Grid>
        </Grid>
  
      </Grid>
    </div>
  );
}
