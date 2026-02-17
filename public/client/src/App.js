import React from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import "./App.css";
// ort 'imp./component/slider/HomeSlider';
import "bootstrap/dist/css/bootstrap.css";
import Home from "./pages/index";
import AboutPage from "./pages/about";
import MembershipPage from "./component/Membership/membership";
import ChamberstructurePage from "./pages/chamberstructure";
import assembly from "./pages/assembly";
import team from "./pages/team";
import board from "./pages/board";
import contact from "./component/ContactUs/contact";
import BusinessDirectory from "./component/BusinessDirectory/BusinessDirectory";
import tradeinvestment from "./pages/trade&investment";
import services from "./component/Services/Services";
import news from "./component/News/News";
import NewDetail from "./component/News/NewsDetail";
import eresources from "./component/Resources/eResources";
import ppDialog from "./component/PPDialog/ppDialog";
import login from "./component/login/Login";
import register from "./component/login/Register";
import ChangePassword from "./component/login/ChangePassword";
import ProfileForm from "./component/login/ProfileForm";
import ServiceRequestForm from "./component/ServiceRequest/ServiceRequestForm";
import MyServicesPage from "./component/Services/MyServicesPage";
import ServiceDetailPage from "./component/ServiceRequest/ServiceDetailPage";
import ExportAgreementDetailPage from "./component/ExportAgreement/ExportAgreementDetailPage";
import SurveyQuestionForm from "./component/Survey/SurveyQuestionForm";
import membership from "./component/Membership/membership";
import events from "./component/Event/Events";
import location from "./pages/location";
import partners from "./pages/partners";
import splan from "./pages/splan";
import chamberlaw from "./pages/Chamberlaw";
import projects from "./pages/Projects";
import MainMenu from "./component/NavMenus/MainMenuy";
import photoGallery from "./component/Gallery/PhotoGallery";
import photoGalleryDetail from "./component/Gallery/PhotoGalleryDetail";
import laws from "./pages/Laws";
import FormerPresident from "./pages/formerPresident";
import { presidents, secretaries } from "./dummyData";
import store from "./store";
import { Provider } from "react-redux";
import AuthProvider from "./utils/authProvider";
import ProtectedRoute from "./utils/protectedRoute";
import LegalProfileForm from "./component/LegalProfile/LegalProfileForm";
import ExportAgreementForm from "./component/ExportAgreement/ExportAgreementForm";
import UserContactForm from "./component/UserContacts/UserContactForm";
import { makeStyles } from "@material-ui/core";
import {
  Button,
  Carousel,
  Col,
  FormControl,
  InputGroup,
  Row,
} from "react-bootstrap";
import MembershipApplyForm from "./component/Membership/MembershipApplyForm";
import ApplicationDetail from "./component/Membership/ApplicationDetail";
import { getAdImages } from "./services/advert";
import { InvestmentProfile } from "./component/InvestmentProfile/InvestmentProfile";
import IssueForm from "./component/ContactUs/IssueForm";
import MatchMaking from "./component/MatchMaking/MatchMaking";
import VerifyCertificate from "./component/ServiceRequest/VerifyCertificate";
import TextMedia from "./component/TextMedia/TextMedia";
import TextMediaDetail from "./component/TextMedia/TextMediaDetail";
import VideoGallery from "./component/Gallery/VideoGallery";
import MenuSearchResult from "./component/NavMenus/MenuSearchResult";
import PayTeleBirr from "./telebirrService/PayTeleBirr";
import ServiceRequestDetailParam from "./component/ServiceRequest/ServiceRequestDetailParam";
import CertificateOfOriginDetailPage from "./component/ServiceRequest/CertificateOfOriginDetailPage";
import FAQ from "./pages/FAQ";
import ResourceRequestDetailPage from "./component/Resources/ResourceRequestDetailPage";
import ResourceRequestDetailParam from "./component/Resources/ResourceRequestDetailParam";
import BusinessInformation from "./component/BusinessInformation/BusinessInformation";
import Footer from "./component/Footer/Footer";
import Bmo from "./pages/bmo";

const useStyles = makeStyles((theme) => ({
  fixedBottom: {
    position: "fixed",
    zIndex: "1",
    width: "100%",
    bottom: "0",
    left: "50%",
    transform: "translate(-50%, 0)",
    maxWidth: "728px",
    overflow: "visible",
    maxHeight: "70px",
  },
}));
const advertHereList = ["ad1.jpg", "ad2.jpg"];
function App() {
  const classes = useStyles();

  const [adImages, setAdImages] = React.useState([]);
  const [isLoggedIn, setIsLoggedIn] = React.useState(false);
  React.useEffect(() => {
    getAdImages(2).then((data) => {
      setAdImages(data.data);
    });
  }, []);

  return (
    <Provider store={store}>
      <AuthProvider store={store}>
        <>
          <Router style={{ flex: 1 }}>
            <div className={`${classes.fixedBottom}`}>
              <Carousel variant="light" indicators={false}>
                {adImages && adImages.$values && adImages.$values.length > 0
                  ? adImages.$values.map((img) => (
                      <Carousel.Item>
                        <img
                          alt={"Advert"}
                          src={
                            process.env.REACT_APP_API_URL +
                            "/AdvertisingImages/footer/" +
                            img
                          }
                          style={{
                            width: "100%",
                            height: "100%",
                            maxHeight: "80px",
                          }}
                        ></img>
                      </Carousel.Item>
                    ))
                  : advertHereList.map((img) => (
                      <Carousel.Item>
                        <img
                          alt={"Advert"}
                          src={"/AdvertisingImages/" + img}
                          style={{
                            width: "100%",
                            height: "100%",
                            maxHeight: "80px",
                          }}
                        ></img>
                      </Carousel.Item>
                    ))}
              </Carousel>
            </div>
            <MainMenu setIsLoggedIn={setIsLoggedIn} />
            <div
              style={{
                marginTop: isLoggedIn ? "178px" : "155px",
                marginBottom: "30px",
              }}
            >
              <Switch>
                <Route exact path="/">
                  <Home />
                </Route>
                <Route path="/membership" component={membership}></Route>
                <Route path="/bmo" component={Bmo}></Route>
                <Route
                  path="/paytelebirr/:returnUrl"
                  component={PayTeleBirr}
                ></Route>
                <Route
                  path="/investmentProfile"
                  component={InvestmentProfile}
                ></Route>
                <Route path="/issueForm" component={IssueForm}></Route>
                <Route path="/matchMaking" component={MatchMaking}></Route>
                <Route
                  path="/verifyCertificate/:code"
                  component={VerifyCertificate}
                ></Route>
                <Route
                  path="/verifyCertificate"
                  component={VerifyCertificate}
                ></Route>
                <ProtectedRoute
                  path="/membershipApplicationForm"
                  component={MembershipApplyForm}
                ></ProtectedRoute>
                <ProtectedRoute
                  path="/membershipRequestDetail"
                  component={ApplicationDetail}
                ></ProtectedRoute>
                <Route path="/mchamberstructure">
                  <ChamberstructurePage />
                </Route>
                <Route path="/faq" component={FAQ}></Route>
                <Route path="/about" component={AboutPage}>
                  <AboutPage />
                </Route>
                <Route path="/former">
                  <FormerPresident
                    presidents={presidents}
                    secretaries={secretaries}
                  />
                </Route>
                <Route path="/picture" component={photoGallery}></Route>
                <Route
                  path="/pictureView"
                  component={photoGalleryDetail}
                ></Route>
                <Route path="/video" component={VideoGallery}></Route>
                <Route path="/laws" component={laws}></Route>
                <Route path="/assembly" component={assembly}></Route>
                <Route path="/team" component={team}></Route>
                <Route path="/partners" component={partners}></Route>
                <Route path="/projects" component={projects}></Route>
                <Route path="/chamberlaw" component={chamberlaw}></Route>
                <ProtectedRoute
                  path="/myServices"
                  component={MyServicesPage}
                ></ProtectedRoute>
                <Route path="/splan" component={splan}></Route>
                <Route path="/register" component={register}></Route>
                <Route path="/board" component={board}></Route>
                <Route path="/contact" component={contact}></Route>
                <Route path="/services" component={services}></Route>
                <Route
                  path="/trade-investment"
                  component={tradeinvestment}
                ></Route>
                <Route path="/login" component={login}></Route>
                <Route
                  path="/businessDirectory"
                  component={BusinessDirectory}
                ></Route>
                <Route
                  path="/businessInformation"
                  component={BusinessInformation}
                ></Route>
                <ProtectedRoute
                  path="/serviceRequestForm"
                  component={ServiceRequestForm}
                ></ProtectedRoute>
                <ProtectedRoute
                  path="/serviceRequestDetail"
                  component={ServiceDetailPage}
                ></ProtectedRoute>
                <ProtectedRoute
                  path="/resourceRequestDetail"
                  component={ResourceRequestDetailPage}
                ></ProtectedRoute>
                <ProtectedRoute
                  path="/certificateOfOriginDetail"
                  component={CertificateOfOriginDetailPage}
                ></ProtectedRoute>
                <ProtectedRoute
                  path="/serviceRequestDetailParam/:serviceTypeId/:requestId"
                  component={ServiceRequestDetailParam}
                ></ProtectedRoute>
                <ProtectedRoute
                  path="/resourceRequestDetailParam/:serviceTypeId/:requestId"
                  component={ResourceRequestDetailParam}
                ></ProtectedRoute>
                <ProtectedRoute
                  path="/surveyQuestionForm"
                  component={SurveyQuestionForm}
                ></ProtectedRoute>
                <ProtectedRoute
                  path="/profileForm"
                  component={ProfileForm}
                ></ProtectedRoute>
                <ProtectedRoute
                  path="/changePassword"
                  component={ChangePassword}
                ></ProtectedRoute>
                <ProtectedRoute
                  path="/legalProfileForm"
                  component={LegalProfileForm}
                ></ProtectedRoute>
                <ProtectedRoute
                  path="/exportAgreementForm"
                  component={ExportAgreementForm}
                ></ProtectedRoute>
                <ProtectedRoute
                  path="/exportAgreementDetail"
                  component={ExportAgreementDetailPage}
                ></ProtectedRoute>
                <ProtectedRoute
                  path="/userContactForm"
                  component={UserContactForm}
                ></ProtectedRoute>
                <Route path="/news" component={news}></Route>
                <Route path="/news_detail" component={NewDetail}></Route>
                <Route path="/location" component={location}></Route>
                <Route path="/events" component={events}></Route>
                <Route path="/eresources" component={eresources}></Route>
                <Route path="/ppDialog" component={ppDialog}></Route>
                <Route path="/textMedias" component={TextMedia}></Route>
                <Route
                  path="/menuSearchResult"
                  component={MenuSearchResult}
                ></Route>
                <Route
                  path="/text_media_detail"
                  component={TextMediaDetail}
                ></Route>
                <Route
                  path="/ChamberstructurePage"
                  component={ChamberstructurePage}
                ></Route>
                <Route path="/membership/chamberstructure">
                  <MembershipPage />
                  <ChamberstructurePage />
                </Route>
                <Route path="/membership">
                  <MembershipPage />
                </Route>
              </Switch>

              <Footer />
            </div>
          </Router>
        </>
      </AuthProvider>
    </Provider>
  );
}

export default App;
