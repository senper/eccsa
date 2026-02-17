import React from 'react';
import { Grid, Breadcrumbs, Link } from '@mui/material';
import {
  getSectorsList,
  getSectorCompanies,
  getCompaniesByKey,
} from '../api/businessInformation';
import CompanyList from '../Components/BusinessDirectory/CompanyList';
import SponsorList from '../Components/BusinessDirectory/SponsorList';
import SectorsList from '../Components/BusinessDirectory/SectorsList';
import SearchForm from '../Components/BusinessDirectory/SearchForm';
import { useRouter } from 'next/router';
import { getLocalByKey } from './api/menu';
import { CommonBreadcrumb } from '../Components/Layout/CommonBreadcrumb';

export default function BusinessDirectory({ translations }) {
  const location = useRouter();
  const { locale } = location;

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == 'en' || !local ? eng : local;
  };

  const [searchKey, setSearchKey] = React.useState('');
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
    setSearchKey('');
    setSearched(false);
    setCompanies([]);
    setLoading(true);
    getSectorCompanies(sector.id)
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
    getCompaniesByKey(searchKey).then((data) => {
      setCompanies(data.data.$values);
      setSearched(true);
      setLoading(false);
    });
  }
  const breadcrumbLinks = () => [
    {
      title: getLocal('Home'),
      path: '/',
    },
    {
      title: getLocal('Business Directory'),
      path: '#',
    },
  ];
  return (
    <>
          <div className="paddedMainDiv">
            <CommonBreadcrumb links={breadcrumbLinks()} />
          </div>
      <Grid spacing={6} container>
        <Grid item xs={5} md={3}>
          <SectorsList
            sectors={sectors}
            loadDirectory={loadDirectory}
            selectedSector={selectedSector}
            counts={counts}
          />
        </Grid>
        <Grid item xs={7} md={9}>
          <SearchForm
            searchSubmit={searchSubmit}
            setSearchKey={setSearchKey}
            searchKey={searchKey}
            getLocal={getLocal}
          />
          {!loading && !selectedSector && !searched ? (
            <SponsorList getLocal={getLocal} />
          ) : (
            <CompanyList
              selectedSector={selectedSector}
              sectors={sectors}
              companies={companies}
              loading={loading}
              getLocal={getLocal}
            />
          )}
          {loading ? <h3>{getLocal('loading')}...</h3> : null}
        </Grid>
      </Grid>
    </>
  );
}
