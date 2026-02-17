import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Grid, Paper, Container } from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.min.css";
import "../../App.css";
import "../slider/HomeSlider";
import "bootstrap/dist/css/bootstrap.css";
import { Link } from "react-router-dom";
import { Breadcrumbs } from "@material-ui/core";
import { useLocation } from "react-router-dom";
import Footer from "../Footer/Footer";
import {
  getAssociation,
  getECCSAAssociations,
} from "../../services/membership";
import AssociationLinks from "./AssociationLinks";
import AssociationInfoCard from "./AssociationInfoCard";
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
  let newsIdValue = 0;
  if (location.state) {
    const { newsId } = location.state;
    newsIdValue = newsId;
  }
  const classes = useStyle();
  const [selectedAssociation, setSelectedAssociation] = React.useState(null);
  const [loadingSelectedAssDetail, setLoadingSelectedAssDetail] =
    React.useState(false);
  const loadAssociation = (id) => {
    setLoadingSelectedAssDetail(true);
    getAssociation(id)
      .then((data) => {
        var res = data.data;
        setSelectedAssociation({
          ...res.association,
          logo: res.logo,
          boardChairman: res.boardChairman,
          ceo: res.ceo,
        });
        setLoadingSelectedAssDetail(false);
      })
      .catch((error) => {
        setLoadingSelectedAssDetail(false);
      });
  };
  const [eccsaAssociations, setECCSAAssociations] = React.useState([]);
  React.useEffect(() => {
    getECCSAAssociations().then((data) => {
      setECCSAAssociations(data.data.$values);
    });
  }, []);

  const [membershipDescription, setMembershipDescription] =
    React.useState(null);
  React.useEffect(() => {
    getServiceOwner(1).then((data) => {
      setMembershipDescription(data.data);
    });
  }, []);
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
          Membership
        </Link>
      </Breadcrumbs>
      <Grid container spacing={2} className="grid">
        <Grid item xs={1} md={1}></Grid>
        <Grid item xs={11} md={8}>
          <Container>
            {selectedAssociation && !loadingSelectedAssDetail ? (
              <>
                <AssociationInfoCard association={selectedAssociation} />
                <br />
              </>
            ) : null}
            {loadingSelectedAssDetail ? (
              <strong>Loading Member Detail ...</strong>
            ) : null}
            <Grid container spacing={1}>
              <Grid item xs={12}>
                <hr style={{ width: "100%", margin: "0", marginTop: "5px" }} />
                <strong>Membership</strong>
                {membershipDescription ? (
                  <pre>{membershipDescription.description}</pre>
                ) : null}
              </Grid>
            </Grid>
          </Container>
        </Grid>
        <Grid item xs={12} md={3}>
          <AssociationLinks
            associations={eccsaAssociations}
            loadAssociation={loadAssociation}
            selectedAssociation={selectedAssociation}
          />
        </Grid>
      </Grid>
    </div>
  );
}

export default App;
