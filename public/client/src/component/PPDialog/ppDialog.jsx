import React from "react";

import { makeStyles } from "@material-ui/core/styles";
import { getDialog } from "../../services/resources";
import { Grid, Paper } from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.min.css";
import "../../App.css";
import "../slider/HomeSlider";
import { Link } from "react-router-dom";
import { Breadcrumbs } from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.css";
import Footer from "../Footer/Footer";
import { useLocation } from "react-router-dom";
import PPDialogCategoryLinks from "./PPDialogCategoryLinks";
import PPDialogIssues from "./PPDialogIssues";
import PPDialogAttachments from "./PPDialogAttachments";
import { getServiceOwner } from "../../services/servicetype";

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
  const [dialog, setDialog] = React.useState(null);
  const [issues, setIssues] = React.useState([]);
  const [documents, setDocuments] = React.useState([]);
  let categoryIdValue = 0;
  if (location.state) {
    const { categoryId } = location.state;
    categoryIdValue = categoryId;
  }
  React.useEffect(() => {
    if (categoryIdValue) {
      getDialog(categoryIdValue).then((data) => {
        setDialog(data.data);
        if (data.data.recommendations && data.data.recommendations.$values) {
          setIssues(data.data.recommendations.$values);
        }
        if (data.data.documents && data.data.documents.$values) {
          setDocuments(data.data.documents.$values);
        }
      });
    }
  }, [categoryIdValue]);

  const [serviceDescription, setServiceDescription] = React.useState(null);
  React.useEffect(() => {
    getServiceOwner(32).then((data) => {
      setServiceDescription(data.data);
    });
  }, []);

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
              to="#"
              aria-current="page"
              onClick={() => setDialog(null)}
            >
              Public Private Dialog
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
                    {dialog ? (
                      <>
                        <h3>{dialog.title}</h3>
                        <PPDialogIssues issues={issues} />
                        <PPDialogAttachments attachments={documents} />
                      </>
                    ) : serviceDescription ? (
                      <>
                        <strong>Advocacy and Research</strong>
                        <br />
                        <pre>{serviceDescription.description}</pre>
                      </>
                    ) : (
                      <h3>No Category Selected</h3>
                    )}
                  </div>
                </div>
              </div>
            </div>
          </Paper>
        </Grid>
        <Grid item xs={12} md={3}>
          <PPDialogCategoryLinks />
        </Grid>
  
      </Grid>
    </div>
  );
}

export default App;
