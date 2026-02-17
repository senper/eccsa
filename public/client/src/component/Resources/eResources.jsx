import React from "react";

import { makeStyles } from "@material-ui/core/styles";
import { getResources, getResource } from "../../services/resources";
import { Grid, Paper } from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.min.css";
import "../../App.css";
import "../slider/HomeSlider";
import { Link } from "react-router-dom";
import { Breadcrumbs } from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.css";
import Footer from "../Footer/Footer";
import ResourceLinks from "./ResourceLinks";
import { useLocation } from "react-router-dom";
import ResourceCard from "./ResourceCard";

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

function App() {
  const location = useLocation();
  const classes = useStyle();
  const [resources, setResources] = React.useState([]);
  const [resourceType, setResourceType] = React.useState([]);
  const [loading, setLoading] = React.useState([]);
  let typeIdValue = 0;
  let typeTextValue = "";
  if (location.state) {
    const { typeId, typeText } = location.state;
    typeIdValue = typeId;
    typeTextValue = typeText;
  }
  React.useEffect(() => {
    if (typeIdValue) {
      setLoading(true);
      setResources([]);
      setResourceType(typeTextValue);
      getResources(typeIdValue)
        .then((data) => {
          setResources(data.data.$values);
          setResourceType(typeTextValue);
          setLoading(false);
        })
        .catch((error) => {
          setLoading(false);
        });
    }
  }, [typeIdValue]);
  const [selectedResource, setSelectedResource] = React.useState(null);
  const loadResource = (value) => {
    if (selectedResource && value.id == selectedResource.id) {
      setSelectedResource(null);
      return;
    }
    setSelectedResource(value);
    /* getResource(value.id).then((data) => {
      setSelectedResource(data.data);
    }); */
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
            <Link color="textPrimary" to="#" aria-current="page">
              e-Resources
            </Link>
          </Breadcrumbs>
        </Grid>
        <Grid item xs={1} md={1}></Grid>
        <Grid item xs={11} md={8}>
          <Paper>
            <div class="row">
              <div class="col-sm-12">
                <div class="card">
                  <div class="card-body">
                    <h3>{typeIdValue != 0 ? resourceType : null}</h3>
                    <ol>
                      {typeIdValue != 0 ? (
                        <>
                          {loading ? <h3>Loading...</h3> : null}
                          {!loading && resources && resources.length > 0 ? (
                            resources.map((value) => (
                              <li key={value.id}>
                                <Link
                                  to={{
                                    state: {
                                      typeId: typeIdValue,
                                      typeText: typeTextValue,
                                    },
                                  }}
                                  onClick={() => loadResource(value)}
                                >
                                  <strong>{value.title}</strong>
                                </Link>
                                {selectedResource &&
                                selectedResource.id === value.id ? (
                                  <>
                                    <ResourceCard resource={selectedResource} />
                                    <a
                                      href="javascript:void(0)"
                                      style={{ margin: "0px 0px 0px 0px" }}
                                      className="btn btn-sm btn-warning"
                                      onClick={() => setSelectedResource(null)}
                                    >
                                      Close
                                    </a>
                                    <br />
                                  </>
                                ) : null}
                              </li>
                            ))
                          ) : !loading ? (
                            <strong>No Resources Found</strong>
                          ) : null}
                        </>
                      ) : (
                        <>
                          <img src="/images/resourceimg1.png" />
                          <br />
                          <img src="/images/resourceimg2.png" />
                        </>
                      )}
                    </ol>
                  </div>
                </div>
              </div>
            </div>
          </Paper>
        </Grid>
        <Grid item xs={12} md={3}>
          <ResourceLinks />
        </Grid>
      </Grid>
    </div>
  );
}

export default App;
