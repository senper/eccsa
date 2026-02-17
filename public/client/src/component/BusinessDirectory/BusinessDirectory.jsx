import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import {
  Grid,
} from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.min.css";
import "../../App.css";
import { Link } from "react-router-dom";
import { Breadcrumbs } from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.css";
import Footer from "../Footer/Footer";
import {
  getSectorsList,
  getSectorCompanies,
  getCompaniesByKey,
} from "../../services/businessInformation";
import CompanyList from "./CompanyList";
import SponsorList from "./SponsorList";
import SectorsList from "./SectorsList";
import SearchForm from "./SearchForm";

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
export default function BusinessDirectory() {
  const classes = useStyle();
  const [searchKey, setSearchKey] = React.useState("");
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
  function loadDirectory(sector) {
    setSelectedSector(sector);
    setSearchKey("");
    setSearched(false);
    setCompanies([]);
    setLoading(true);
    getSectorCompanies(sector.id).then((data) => {
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
    getCompaniesByKey(searchKey).then((data) => {
      setCompanies(data.data.$values);
      setSearched(true);
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
              Business Directory
            </Link>
          </Breadcrumbs>
          <SectorsList
            sectors={sectors}
            loadDirectory={loadDirectory}
            selectedSector={selectedSector}
            counts={counts}
          />
        </Grid>
        <Grid item xs={9} md={9}>
          <SearchForm searchSubmit={searchSubmit} setSearchKey={setSearchKey}
          searchKey={searchKey} />          
          {!loading && !selectedSector && !searched ? (
            <SponsorList />
          ) : (
            <CompanyList
              selectedSector={selectedSector}
              sectors={sectors}
              companies={companies}
              loading={loading}
            />
          )}
          {loading ? <h3>Loading...</h3> : null}
        </Grid>
      </Grid>
    </div>
  );
}
