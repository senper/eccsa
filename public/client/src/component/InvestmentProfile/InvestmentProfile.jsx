import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Grid, Container } from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.min.css";
import "../../App.css";
import "../slider/HomeSlider";
import "bootstrap/dist/css/bootstrap.css";
import { Link } from "react-router-dom";
import { Breadcrumbs } from "@material-ui/core";
import { useLocation } from "react-router-dom";
import Footer from "../Footer/Footer";
import InvestmentProfileLinks from "./InvestmentProfileLinks";
import InvestmentProfileDetail from "./InvestmentProfileDetail";
import {
  getInvestmentProfile,
  getInvestmentProfilesList,
} from "../../services/businessInformation";

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
const EthiopiaId = "00000000-0000-0000-0000-0000000f4240";
export function InvestmentProfile() {
  const location = useLocation(); ///
  let typeIdValue = 0;

  if (location.state) {
    const { typeId } = location.state;
    typeIdValue = typeId;
  }
  const classes = useStyle();
  const [loading, setLoading] = React.useState(false);
  const [profileType, setProfileType] = React.useState(null);
  React.useEffect(() => {
    if (typeIdValue) {
      changeType(typeIdValue);
    }
  }, [location.state]);
  const [selectedInvestmentProfile, setSelectedInvestmentProfile] =
    React.useState(null);
  const loadInvestmentProfile = (id) => {
    setLoading(true);
    getInvestmentProfile(id)
      .then((data) => {
        setSelectedInvestmentProfile(data.data);
        setLoading(false);
      })
      .catch((error) => {
        setLoading(false);
      });
  };
  const [investmentProfiles, setInvestmentProfiles] = React.useState([]);
  const [filteredList, setFilteredList] = React.useState([]);
  React.useEffect(() => {
    getInvestmentProfilesList().then((data) => {
      setInvestmentProfiles(data.data.$values);
      const list = getFilteredList(
        data.data.$values,
        profileType ? profileType : typeIdValue
      );

      setFilteredList([...list]);
      if (list && list.length > 0) {
        loadInvestmentProfile(list[0].id);
      }
    });
  }, []);

  function getProfileListLabel() {
    return profileType == 3
      ? "Ethiopia"
      : profileType == 1
      ? "Regions"
      : "Sectors";
  }

  function changeType(type) {
    setProfileType(type);
    const list = getFilteredList(investmentProfiles, type);
    setFilteredList([...list]);
    if (list.length > 0) {
      loadInvestmentProfile(list[0].id);
    }
  }
  function getFilteredList(mainList, type) {
    const list = mainList
      ? mainList.filter(
          (l) =>
            (type == 3 && l.id == EthiopiaId) ||
            (type == 1 && l.id != EthiopiaId && l.isRegion) ||
            (type == 2 && l.id != EthiopiaId && !l.isRegion)
        )
      : [];
    return list;
  }
  return (
    <div class="mmenu">
      <Grid container spacing={2} className="grid">
        <Grid item xs={12} md={12}>
          <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrum}>
            <Link color="inherit" to="/">
              Home
            </Link>

            <Link color="textPrimary" to="#" aria-current="page">
              Investment Profile
            </Link>
          </Breadcrumbs>
        </Grid>
        <Grid item xs={1} md={1}></Grid>
        <Grid item xs={11} md={8}>
          <Container>
            {loading ? <h3>Loading ...</h3> : null}
            {!loading && selectedInvestmentProfile ? (
              <InvestmentProfileDetail
                profileEntity={selectedInvestmentProfile}
              />
            ) : null}
          </Container>
        </Grid>
        <Grid item xs={12} md={3}>
          {profileType != 3 ? (
            <>
              <Link onClick={() => changeType(3)}>Change to Ethiopia</Link>
              <br />
            </>
          ) : null}
          {profileType != 1 ? (
            <>
              <Link onClick={() => changeType(1)}>Change to Region</Link>
              <br />
            </>
          ) : null}
          {profileType != 2 ? (
            <>
              <Link onClick={() => changeType(2)}>Change to Sector</Link>
              <br />
            </>
          ) : null}
          <InvestmentProfileLinks
            investmentProfiles={filteredList}
            loadInvestmentProfileDetail={loadInvestmentProfile}
            selectedInvestmentProfile={selectedInvestmentProfile}
            profileListTitle={getProfileListLabel()}
          />
        </Grid>
      </Grid>
    </div>
  );
}
