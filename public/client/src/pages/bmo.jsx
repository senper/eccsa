import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Grid, Paper } from "@material-ui/core";
import { Link } from "react-router-dom";
import Breadcrumbs from "@material-ui/core/Breadcrumbs";
import "bootstrap/dist/css/bootstrap.min.css";
import "../App.css";
import "../component/slider/HomeSlider";
import "bootstrap/dist/css/bootstrap.css";
import { getTextMediaList } from "../services/otherMedia";
import BMOCard from "../component/BMO/BMOCard";

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
    marginLeft: "100px",
  },
}));

export default function Bmo() {
  const classes = useStyle();
  const mediaCategoryValue = 3;
  const [bmoList, setBMOList] = React.useState([]);
  const [loading, setLoading] = React.useState([]);
  React.useEffect(() => {
    if (mediaCategoryValue) {
      setLoading(true);
      getTextMediaList(mediaCategoryValue)
        .then((data) => {
          setBMOList(data.data.$values);
          setLoading(false);
        })
        .catch((error) => {
          setLoading(false);
        });
    }
  }, []);
  const [selectedBMO, setSelectedBMO] = React.useState(null);
  const loadBMO = (value) => {
    if (selectedBMO && value.id == selectedBMO.id) {
      setSelectedBMO(null);
      return;
    }
    setSelectedBMO(value);
  };
  return (
    <div class="mmenu">
      <Grid container spacing={2} className="grid">
        <Grid item xs={12} md={12}>
          <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrum}>
            <Link to="/"> Home</Link>
            <Link eventKey="bmo" aria-current="page" to="#">
              {" "}
              BMO
            </Link>
          </Breadcrumbs>
        </Grid>{" "}
        <Grid item xs={1} md={1}></Grid>
        <Grid item xs={11} md={10}>
          <Paper>
            <div class="row">
              <div class="col-sm-12">
                <div class="card">
                  <div class="card-body">
                    <h3>Business Membership Organizations</h3>
                    <ol>
                          {loading ? <h3>Loading...</h3> : null}
                          {!loading && bmoList && bmoList.length > 0 ? (
                            bmoList.map((value, index) => (
                              <li key={index}>
                                <Link
                                  onClick={() => loadBMO(value)}
                                >
                                  <strong>{value.titleFull}</strong>
                                </Link>
                                {selectedBMO &&
                                selectedBMO.id === value.id ? (
                                  <>
                                    <BMOCard bmo={selectedBMO} />
                                    <a
                                      href="javascript:void(0)"
                                      style={{ margin: "0px 0px 0px 0px" }}
                                      className="btn btn-sm btn-warning"
                                      onClick={() => setSelectedBMO(null)}
                                    >
                                      Close
                                    </a>
                                    <br />
                                  </>
                                ) : null}
                              </li>
                            ))
                          ) : !loading ? (
                            <strong>Noting found Found</strong>
                          ) : null}
                    </ol>
                  </div>
                </div>
              </div>
            </div>
          </Paper>
        </Grid>
      </Grid>
    </div>
  );
}
