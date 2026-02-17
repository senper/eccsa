import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Grid } from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.min.css";
import "../../App.css";
import { Link, useLocation } from "react-router-dom";
import { Breadcrumbs } from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.css";
import Footer from "../Footer/Footer";
import CompanyList from "./MMCompanyList";
import SectorsList from "./MMSectorsList";
import SearchForm from "./MMSearchForm";
import {
  getSectorsList,
  getSourcesList,
  getUserMatches,
  searchSourcesList,
} from "../../services/businessInterest";
import { getLookupValues } from "../../services/contactUs";

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
export default function MatchMaking() {
  const location = useLocation();
  const classes = useStyle();
  var token = localStorage.getItem("access_token");
  const [userMatches, setUserMatches] = React.useState([]);
  React.useEffect(() => {
    loadUserMatches();
  }, [token]);
  function loadUserMatches() {
    if (token) {
      getUserMatches().then((data) => {
        setUserMatches(data.data.$values);
      });
    }
  }
  const [searchKey, setSearchKey] = React.useState("");
  const [selectedBusinessType, setSelectedBusinessType] = React.useState(null);
  const [businessTypes, setBusinessTypes] = React.useState([]);
  React.useEffect(() => {
    getLookupValues("00000000-0000-0000-0000-00000000002e").then((data) => {
      setBusinessTypes(data.data.$values);
      if (data.data.$values && data.data.$values.length > 0) {
        setSelectedBusinessType("00000000-0000-1000-0000-000000000022");
      }
    });
  }, []);
  const [searched, setSearched] = React.useState(false);
  const [sectors, setSectors] = React.useState([]);
  const [counts, setCounts] = React.useState([]);
  const [companies, setCompanies] = React.useState([]);
  const [loading, setLoading] = React.useState(false);
  const [selectedSector, setSelectedSector] = React.useState(null);
  React.useEffect(() => {
    getSectorsList().then((data) => {
      setSectors(data.data.sectors.$values);
      setCounts(data.data.counts.$values);
    });
  }, []);
  let sectorIdValue = 0;
  let subSectorIdValue = null;
  if (location.state) {
    const { sectorId, subSectorId } = location.state;
    sectorIdValue = sectorId;
    subSectorIdValue = subSectorId;
  }
  React.useEffect(() => {
    console.log(subSectorIdValue, sectors);
    if (subSectorIdValue && sectors.find((s) => s.id == subSectorIdValue)) {
      loadDirectory(sectors.find((s) => s.id == subSectorIdValue));
    }
  }, [subSectorIdValue, sectors]);
  function loadDirectory(sector) {
    setSelectedSector(sector);
    setSearchKey("");
    setSearched(false);
    setCompanies([]);
    setLoading(true);
    getSourcesList(sector.id)
      .then((data) => {
        setCompanies(data.data.$values);
        setLoading(false);
      })
      .catch((error) => {
        setLoading(false);
      });
  }
  function searchSubmit(event) {
    event.preventDefault();
    setSelectedSector(null);
    setSearched(false);
    if (!searchKey || searchKey.trim().length <= 0) {
      return;
    }
    setCompanies([]);
    setLoading(true);
    searchSourcesList(selectedBusinessType, searchKey)
      .then((data) => {
        setCompanies(data.data.$values);
        setSearched(true);
        setLoading(false);
      })
      .catch((error) => {
        setLoading(false);
      });
  }
  return (
    <div class="mmenu">
      <Grid container spacing={2} className="grid">
        <Grid item xs={3} md={3}>
          <Breadcrumbs
            aria-label="breadcrumb"
            className={classes.breadcrum}
            style={{ marginBottom: "30px" }}
          >
            <Link color="textPrimary" to="#" aria-current="page">
              Services
            </Link>
            <Link color="textPrimary" to="#" aria-current="page">
              Match Making
            </Link>
          </Breadcrumbs>
          <SectorsList
            sectors={sectors}
            loadDirectory={loadDirectory}
            selectedSector={selectedSector}
            counts={counts}
            sectorId={sectorIdValue}
          />
        </Grid>
        <Grid item xs={9} md={9}>
          <SearchForm
            searchSubmit={searchSubmit}
            setSearchKey={setSearchKey}
            searchKey={searchKey}
            selectedBusinessType={selectedBusinessType}
            setSelectedBusinessType={setSelectedBusinessType}
            businessTypes={businessTypes}
          />
          {!loading && !selectedSector && !searched ? (
            <>
              <hr style={{ width: "100%" }} />
              <h3>Select Sector to get list of Companies</h3>
            </>
          ) : (
            <CompanyList
              selectedSector={selectedSector}
              sectors={sectors}
              companies={companies}
              loading={loading}
              businessTypes={businessTypes}
              loadUserMatches={loadUserMatches}
              userMatches={userMatches}
              searched={searched}
            />
          )}
          {loading ? <h3>Loading...</h3> : null}
        </Grid>
      </Grid>
    </div>
  );
}
