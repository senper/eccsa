import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Grid, Container } from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.min.css";
import "../../App.css";
import "../slider/HomeSlider";
import "bootstrap/dist/css/bootstrap.css";
import { Link } from "react-router-dom";
import { Breadcrumbs } from "@material-ui/core";
import BusinessInfoNewsCard from "./BusinessInfoNewsCard";
import { useLocation } from "react-router-dom";
import {
  getBusinessInformation,
  getBusinessInformationList,
  getPastBusinessInformationList,
} from "../../services/businessInformation";
import Footer from "../Footer/Footer";

const useStyle = makeStyles((theme) => ({
  grid: {
    width: "100%",
    margin: "0px",
    padding: "3px",
  },
  breadcrum: {
    backgroundColor: "#fff",
    padding: "10px",
    marginLeft: "100px",
  },
}));

export default function BusinessInformation() {
  const location = useLocation();
  const classes = useStyle();
  let newsIdValue = 0;
  if (location.state) {
    const { newsId } = location.state;
    newsIdValue = newsId;
  }
  const [loadingCurr, setLoadingCurr] = React.useState(false);
  const [loadingPast, setLoadingPast] = React.useState(false);
  const [news, setNews] = React.useState([]);
  React.useEffect(() => {
    setLoadingCurr(true);
    getBusinessInformationList()
      .then((data) => {
        setNews(data.data.$values);
        setLoadingCurr(false);
      })
      .catch((error) => {
        setLoadingCurr(false);
      });
  }, []);
  const [pastNews, setPastNews] = React.useState([]);
  React.useEffect(() => {
    setLoadingPast(true);
    getPastBusinessInformationList()
      .then((data) => {
        setPastNews(data.data.$values);
        setLoadingPast(false);
      })
      .catch((error) => {
        setLoadingPast(false);
      });
  }, []);
  const [selectedPastNews, setSelectedPastNews] = React.useState(null);
  const loadNews = (value) => {
    getBusinessInformation(value.id).then((data) => {
      setSelectedPastNews(data.data);
    });
  };

  function compare2(a, b) {
    if (newsIdValue === a.id) {
      return -1;
    }
    if (newsIdValue === b.id) {
      return 1;
    }
    return 0;
  }
  return (
    <div class="mmenu">
      <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrum}>
        <Link color="inherit" to="/">
          Home
        </Link>

        <Link color="textPrimary" to="#" aria-current="page">
          Business Information
        </Link>
      </Breadcrumbs>
      <Grid container spacing={2} className="grid">
        <Grid item xs={1} md={1}></Grid>
        <Grid item xs={11} md={8}>
          <Container>
            <div className="bg" style={{ marginTop: "0" }}>
              {!loadingCurr && news.length <= 0
                ? "No Current Business Information Found"
                : "Current Business Information"}
            </div>
            <Grid container spacing={1}>
              {loadingCurr ? <h5>loading ...</h5> : null}
              {news.sort(compare2).map((value) => (
                <BusinessInfoNewsCard news={value} />
              ))}
            </Grid>
            <div className="bg">
              {!loadingPast && pastNews.length <= 0
                ? "No Past Business Information Found"
                : "Past Business Information"}
            </div>
            <Grid container spacing={1}>
              {selectedPastNews ? (
                <>
                  <BusinessInfoNewsCard news={selectedPastNews} />
                  <Grid item xs={12}>
                    <Link
                      style={{ margin: "0px 0px 0px 0px" }}
                      className="btn btn-sm btn-warning"
                      onClick={() => setSelectedPastNews(null)}
                    >
                      Close
                    </Link>
                  </Grid>
                </>
              ) : null}
              <Grid item xs={12}>
                {loadingPast ? <h5>loading ...</h5> : null}
                {pastNews.map((value) => (
                  <>
                    <Link onClick={() => loadNews(value)}>{value.title}</Link>
                    &nbsp;&nbsp;&nbsp;{"  "}
                  </>
                ))}
              </Grid>
            </Grid>
          </Container>
        </Grid>
      </Grid>
    </div>
  );
}
